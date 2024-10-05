using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace Model
{
    public class TaskFlowContext : IdentityDbContext<AppUser>
    {
        public TaskFlowContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<TaskPriority> TaskPriorities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.TaskCategory)
                .WithMany(tc => tc.TaskItems)
                .HasForeignKey(t => t.TaskCategoryId)
                .IsRequired();

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.TaskPriority)
                .WithMany(tp => tp.TaskItems)
                .HasForeignKey(t => t.TaskPriorityId);

            modelBuilder.Entity<TaskCategory>()
                .HasOne(tc => tc.Owner)
                .WithMany(u => u.TaskCategories)
                .HasForeignKey(tc => tc.OwnerId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}