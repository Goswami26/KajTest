using System.ComponentModel.DataAnnotations;

namespace KajTest.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
       
        public string ? UserName { get; set; } 
       
        public string ? UserEmail { get; set; } 
       
        public string ? Mobile { get; set; } 
        public string? HashedPassword { get; set; } 

        // Relation With Project
        public ICollection<Project> ? Projects { get; set; }

        // Relation with Comment
        public ICollection<Comment> ? Comments { get; set; }

        // Relation with ProjectMember
        public ICollection<ProjectMember> ? ProjectMembers { get; set; }

        // Relation with TaskAssignment
        public ICollection<TaskAssinment> ? TaskAssinments { get; set; }

        // Relation with UserRole
        public ICollection<UserRole>? UserRoles { get; set; }

        // Relation with TaskItem
        public ICollection<TaskItem>? TaskItems { get; set; }
    }
}
