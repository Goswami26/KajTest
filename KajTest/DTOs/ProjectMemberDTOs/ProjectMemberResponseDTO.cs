using KajTest.Models;

namespace KajTest.DTOs.ProjectMemberDTOs
{
    public class ProjectMemberResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime JoinOn { get; set; } 
    }
}
