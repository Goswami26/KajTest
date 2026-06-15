namespace KajTest.Models
{
    public class ProjectMember
    {
        public int UserId { get; set; }
        public User ? User { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        // Roles
        public string Role { get; set; } = string.Empty;
        public DateTime JoinOn { get; set; } = DateTime.UtcNow;
    }
}
