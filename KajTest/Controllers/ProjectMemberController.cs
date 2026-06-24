using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using KajTest.Data;
using KajTest.DTOs.ProjectMemberDTOs;
using KajTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KajTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMemberController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProjectMemberController(AppDbContext dbContext)
        {
            this ._dbContext = dbContext;
        }

        //get all Members by Project ID
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult> GetAllMembers(int id)
        {
            //userid
            var userid = GetUserId();

            //member or not
            var ismember = await CheckifMember(id, userid);

            if (!ismember)
            {
                return Forbid();
            }

            //find Other member
            var members = await _dbContext.ProjectMembers
                .Where(pm => pm.ProjectId == id)
                .Select(pm => new ProjectMemberResponseDTO
                {
                    UserId = pm.UserId,
                    Username = pm.User!.UserName!,
                    Role = pm.Role,
                    JoinOn = pm.JoinOn
                })
                .ToListAsync();

            return Ok(members);
        }

        //Add Members
        [HttpPost("{id:int}")]
        [Authorize]
        public async Task<ActionResult> AddMember([FromRoute]int id, [FromBody]ProjectMemberDTO dto)
        {
            //userid
            var userId = GetUserId();

            //check is the user is project creator or not
            var isAdmin = await ChecckIfProjectAdmin(id, userId);

            //if not then forbiden
            if (!isAdmin) 
            { 
                return Forbid();
            }

            //add projectmember
            var alreadyexist = await _dbContext.ProjectMembers.AnyAsync(pm => pm.UserId == dto.UserId && pm.ProjectId == id);

            if (alreadyexist) 
            {
                return BadRequest("Already Existing");
            }

            var member = new ProjectMember
            {
                UserId = dto.UserId,
                ProjectId = id,
                Role = "Member",
                JoinOn = DateTime.UtcNow
            };

            await _dbContext.ProjectMembers.AddAsync(member);
            await _dbContext.SaveChangesAsync();

            return Ok(member);
        }


        //Privat Helpers
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        private async Task<bool> CheckifMember(int projectId, int userId)
        {
            return await _dbContext.ProjectMembers.AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
        }

        private async Task<bool> ChecckIfProjectAdmin(int projectId, int userId)
        {

            return await _dbContext.ProjectMembers.AnyAsync(pm => pm.UserId == userId && pm.ProjectId == projectId && pm.Role == "ProjectAdmin");
        }

    }
}
