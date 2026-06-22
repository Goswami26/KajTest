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

            var project = new Project
            {
                ProjectName = dto.ProjectName,
                ProjectDescription = dto.Description,
                CreatorId = userid,
            };

            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();

            var member = new ProjectMember
            {
                UserId = userid,
                ProjectId = project.ProjectId,
                Role = "ProjectAdmin",
                JoinOn = DateTime.UtcNow
            };

            await _db.ProjectMembers.AddAsync(member);

            await _db.SaveChangesAsync();

            return Ok(new {messege = "Project Create",project.ProjectId });
        }



        //update project
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult> UpdateProjectById(int id, CreateProjectDTO dto)
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var project = await _db.Projects.FindAsync(id);
                
            if (project == null)
            {                
                return NotFound();
            }

            if (project.CreatorId != userid && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            project.ProjectName = dto.ProjectName;
            project.ProjectDescription = dto.Description;

            await _db.SaveChangesAsync();

            return Ok(project);
        }


        //delete project
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult> DeleteProjectById(int id)
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var project = await _db.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            if (project.CreatorId != userid && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _db.Projects.Remove(project);

            await _db.SaveChangesAsync();

            return Ok("Project Deleted");
        }


        //get project
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult> GetProjectById(int id)
        {
            var project = await _db.Projects.AsNoTracking()
                .Select(p => new ProjectResponseDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    CreatorName = p.User.UserName,
                })
                .FirstOrDefaultAsync(p => p.ProjectId == id);
            //var project = await _db.Projects.AsNoTracking.FindAsync(id);

            if (project == null)
            {
                //return Ok("Project Not Found");
                return NotFound();

            }
            return Ok(project);
        }

        //get all project
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult> GetProjects(int id)
        {
            var projects = await _db.Projects.AsNoTracking()
                .Select(p => new ProjectResponseDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    CreatorName = p.User.UserName,
                })
                .ToListAsync();
            //var project = await _db.Projects.AsNoTracking.FindAsync(id);

            if (projects == null)
            {
                //return Ok("Project Not Found");
                return NotFound();

            }
            return Ok(projects);
        }
    }
}
