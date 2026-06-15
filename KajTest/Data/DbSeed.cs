using Microsoft.EntityFrameworkCore;
using System.Data;
using KajTest.Models;
namespace KajTest.Data
{
    public class DbSeed
    {
        public static async Task seedAsync(AppDbContext _db)
        {
            await _db.Database.MigrateAsync();

            // Seed Role
            if (!_db.Roles.Any())
            {
                var roles = new List<Role> {

                  new Role{RoleName = "Admin"},
                  new Role{RoleName = "Devloper"},
                  new Role{RoleName = "Manager"}
                };

                await _db.AddRangeAsync(roles);
                await _db.SaveChangesAsync();
            }

            // Seed User
            if (!_db.Users.Any())
            {
                var admin = new User
                {
                    UserName = "Admin",
                    UserEmail = "admin@gmail.com",
                    Mobile = "8965147325"
                };

                await _db.AddAsync(admin);

                var adminRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");

                var newUserRole = new UserRole
                {
                    User = admin,
                    Role = adminRole
                };

                await _db.UserRoles.AddAsync(newUserRole);
                await _db.SaveChangesAsync();
            }


        }
    }
}
