using System.ComponentModel.DataAnnotations;

namespace KajTest.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string ? RoleName { get; set; }

        // Userrole Reationship
        public ICollection<UserRole>? UserRoles
        { get; set; }
    }
}
