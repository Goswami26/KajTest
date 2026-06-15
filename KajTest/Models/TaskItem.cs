using System.ComponentModel.DataAnnotations;

namespace KajTest.Models
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
      
        public string ? Title { get; set; } 
       
        public string ? Description { get; set; } 
        public DateTime DueDate { get; set; }
        public String Status { get; set; } = string.Empty;

        // Relation with User
        public int UserId { get; set; }
        public User? User { get; set; }

        // Relation with Project
        public int ProjectId { get; set; }
        public Project ? Project { get; set; }

        // Relation with Comment
        public ICollection<Comment> ? Comments { get; set; }

        // Relation with TaskAssignment
        public ICollection<TaskAssinment>? TaskAssinments { get; set; }
    }
}
