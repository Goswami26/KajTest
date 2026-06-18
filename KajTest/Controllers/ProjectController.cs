using System.Security.Claims;
using KajTest.Data;
using KajTest.DTOs.ProjectDtos;
using KajTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KajTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProjectController(AppDbContext _db)
        {
            this._db = _db;
        }

        //create Project
        [HttpPost("create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Create(CreateProjectDTO dto)
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            //if(userid == null)
            //{
            //    return Unauthorized();
            //}

            //var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);

            var project = new Project
            {
                ProjectName = dto.ProjectName,
                ProjectDescription = dto.Description,
                CreatorId = userid,
            };

            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();

            return Ok(new {messege = "Project Create",project.ProjectId });
        }



        //update project

        //delete project

        //get project
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult> GetProjectById(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);

            if(project == null)
            {
                return Ok("Project Not Found");
            };



            return Ok("check");
        }

        //get all project
    }
}
