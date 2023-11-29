namespace Taskedo.Tasks.Database.EF;

public class TasksContext : DbContext
{
    public DbSet<TaskDbEntity> Tasks { get; set; }

    public TasksContext(DbContextOptions<SqlServerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _mapper.Map(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}