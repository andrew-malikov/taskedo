using Microsoft.EntityFrameworkCore;

namespace Taskedo.Tasks.Database.EF;

public class TasksContext : DbContext
{
    public DbSet<TaskDbEntity> Tasks { get; set; }

    public TasksContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskDbEntity>().ToTable("Task");
        modelBuilder.Entity<TaskDbEntity>().Property(t => t.TaskId).HasColumnName("TaskId");
        modelBuilder.Entity<TaskDbEntity>().HasKey(t => t.TaskId);
        modelBuilder.Entity<TaskDbEntity>().Property(t => t.Title).HasColumnName("Title").HasMaxLength(255);
        modelBuilder.Entity<TaskDbEntity>().Property(t => t.Description).HasColumnName("Description").HasMaxLength(2047).HasDefaultValue("");
        modelBuilder.Entity<TaskDbEntity>().Property(t => t.IsCompleted).HasColumnName("IsCompleted");
        modelBuilder.Entity<TaskDbEntity>().Property(t => t.DueDateAtUtc).HasColumnName("DueDateAtUtc");
        modelBuilder.Entity<TaskDbEntity>().Property(t => t.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
        modelBuilder.Entity<TaskDbEntity>().HasIndex(t => t.IsDeleted, "IsDeleted");
        modelBuilder.Entity<TaskDbEntity>().HasQueryFilter(t => !t.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}