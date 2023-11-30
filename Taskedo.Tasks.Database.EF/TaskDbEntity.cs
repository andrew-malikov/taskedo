namespace Taskedo.Tasks.Database.EF;

public class TaskDbEntity
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDateAtUtc { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
}
