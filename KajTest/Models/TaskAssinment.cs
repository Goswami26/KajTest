namespace KajTest.Models
{
    public class TaskAssinment
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }

        public DateTime JoinOn { get; set; } = DateTime.UtcNow;
    }
}
