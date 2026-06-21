using KajTest.Models;

namespace KajTest.DTOs.ProjectMemberDTOs
{
    public class ProjectMemberDTO
    {
        public int UserId { get; set; }
        // Roles
        public string Role { get; set; } = "Member";
        
    }
}
