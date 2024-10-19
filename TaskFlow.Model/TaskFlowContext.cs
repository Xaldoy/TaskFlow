using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using TaskFlow.Model.Models;

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
        public DbSet<FriendRelation> FriendRelations { get; set; }

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

            modelBuilder.Entity<FriendRelation>()
                .HasOne(fr => fr.User1)
                .WithMany(u1 => u1.FriendRelationsAsUser1)
                .HasForeignKey(fr => fr.User1Id)
                .IsRequired();

            modelBuilder.Entity<FriendRelation>()
               .HasOne(fr => fr.User2)
               .WithMany(u1 => u1.FriendRelationsAsUser2)
               .HasForeignKey(fr => fr.User2Id)
               .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}