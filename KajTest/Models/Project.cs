using System.ComponentModel.DataAnnotations;

namespace KajTest.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
 
        public string ? ProjectName { get; set; } 
      
        public string ? ProjectDescription { get; set; } 

        // Owenership
        public int CreatorId { get; set; }
        public User ? User { get; set; }

        // Relation with ProjectMember
        public ICollection<ProjectMember>? ProjectMembers { get; set; }

        // Relation with ProjectMember
        public ICollection<TaskItem>? TaskItems { get; set; }
    }
}
