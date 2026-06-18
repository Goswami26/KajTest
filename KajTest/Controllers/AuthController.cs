using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KajTest.Models;
using KajTest.Data;
using Microsoft.EntityFrameworkCore;
using KajTest.DTOs.AuthDtos;
using KajTest.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

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

            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.Role!.RoleName)
                .ToListAsync();

            //List<string> roles = ["Admin", "Devloper", "Manager"];
            string token = _jwthelper.GenerateToken(user, roles);

            var response = new AuthResponseDTO
            {
                Token = token,
                Username = user.UserName!,
                Email = user.UserEmail!,
                ExpiresIn = DateTime.UtcNow.AddMinutes(1),
                Roles = roles!
            };


            return Ok(response);
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

            var defaultRole = await _db.Roles.FirstOrDefaultAsync(role => role.RoleName == "Devloper");

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
        [HttpGet("me")]
        public async Task<ActionResult> GetCurrentUser() 
        {
            //token -> userid

            var userid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if(userid == null)
            {
                return Unauthorized();
            }

            int id = int.Parse(userid.Value);

            var user = await _db.Users.FirstOrDefaultAsync(user => user.UserId == id);

            //return Ok(userid.Value);
            return Ok(user);
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
