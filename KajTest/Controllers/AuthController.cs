using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KajTest.Models;
using KajTest.Data;
using Microsoft.EntityFrameworkCore;
using KajTest.DTOs.AuthDtos;
using KajTest.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace KajTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtHelpers _jwthelper;

        public AuthController(AppDbContext db, JwtHelpers jwthelper)
        {
            this ._db = db;
            this._jwthelper = jwthelper;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.UserName == loginDTO.UserName);

            if (user == null) {

                return BadRequest("User Does not Exist");
            }           

            if (!VarifyPassward(loginDTO.Password, user.HashedPassword)) 
            {
                return BadRequest("Username or Passward Invalid");
            }

            List<string> roles = ["Admin", "Devloper", "Manager"];
            string token = _jwthelper.GenetereToken(user, roles);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO userDTO)
        {
            //if(user == null)
            //{
            //    return Ok("All Fielse are required");
            //}

            // unique email
            if (await _db.Users.AnyAsync(u => u.UserEmail == userDTO.Email))
            {
                return BadRequest("User Allready Exist");
            }

            // unique mobile
            if (await _db.Users.AnyAsync(u => u.Mobile == userDTO.Mobile))
            {
                return BadRequest("User Allready Exist");
            }            

            var user = new User
            {
                UserName = userDTO.Name,
                UserEmail = userDTO.Email,
                Mobile = userDTO.Mobile,
                HashedPassword = HashPassward(userDTO.Password),
            };

            await _db.Users.AddAsync(user);

            var defaultRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "Devloper");

            if (defaultRole != null) 
            {
                var newuserRole = new UserRole
                {
                    User = user,
                    Role = defaultRole
                };
                
                await _db.UserRoles.AddAsync(newuserRole);

            }

            await _db.SaveChangesAsync();

            return Ok(user);
        }

        [Authorize]
        [HttpGet("test")]
        public string Test() 
        {
            return "It is Working";
        }

        private string HashPassward(string Passward)
        {
            return BCrypt.Net.BCrypt.HashPassword(Passward);
        }

        private bool VarifyPassward(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        } 
    }
}
