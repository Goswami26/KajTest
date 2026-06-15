using KajTest.Models;
using Microsoft.EntityFrameworkCore;
namespace KajTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TaskAssinment> TaskAssinments { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // user
            modelBuilder.Entity<User>(entity =>
            {

                entity.HasKey(u => u.UserId);

                entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property( u => u.UserEmail)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(u => u.Mobile)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

                // Unique constraints (very important in real apps)
                entity.HasIndex(u => u.UserEmail).IsUnique();
                entity.HasIndex(u => u.Mobile).IsUnique();


            });

            // Project
            modelBuilder.Entity<Project>(entity => {


                entity.HasKey(p => p.ProjectId);

                entity.Property(p => p.ProjectName)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(p => p.ProjectDescription)
                .IsRequired()
                .HasMaxLength(1000);

                // Relation With User
                entity.HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            // TaskItem
            modelBuilder.Entity<TaskItem>(entity => {


                entity.HasKey(t => t.TaskItemId);

                entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

                // Relation with User
                entity.HasOne(t => t.User)
               .WithMany(u => u.TaskItems)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);

                // Relation with Project
                entity.HasOne(t => t.Project)
                .WithMany(p => p.TaskItems)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            // Comment
            modelBuilder.Entity<Comment>(entity => {


                entity.HasKey(c => c.CommentId);

                entity.Property(c => c.Text)
                .IsRequired()
                .HasMaxLength(2000);


                // Relation with User
                entity.HasOne(c => c.user)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

                // Relation with Task
                entity.HasOne(c => c.TaskItem)
               .WithMany(t => t.Comments)
               .HasForeignKey(c => c.TaskId)
               .OnDelete(DeleteBehavior.Cascade);

            });

            // ProjectMember
            modelBuilder.Entity<ProjectMember>(entity =>
            {

                entity.HasKey(pm => new {pm.UserId, pm.ProjectId});

                entity.Property(pm => pm.Role)
                .IsRequired()
                .HasMaxLength(100);

                // Relation with Project
                entity.HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);


                // Relation with User
                entity.HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMembers)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            // TaskAssignement
            modelBuilder.Entity<TaskAssinment>(entity =>
            {

                entity.HasKey(ta => new { ta.UserId, ta.TaskItemId });

                // Relation with User
                entity.HasOne(ta => ta.User)
                .WithMany(u => u.TaskAssinments)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);


                // Relation with TaskItem
                entity.HasOne(ta => ta.TaskItem)
                .WithMany(t => t.TaskAssinments)
                .HasForeignKey(ta => ta.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            });


            // Role
            modelBuilder.Entity<Role>(entity => {

                entity.HasKey(r => r.RoleId);

                entity.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(100);
            
            });

            // UserRole
            modelBuilder.Entity<UserRole>(entity => {

                entity.HasKey(ur => new {ur.UserId, ur.RoleId });


                // Relation with User
                entity.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);


                // Relation with role
                entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}
