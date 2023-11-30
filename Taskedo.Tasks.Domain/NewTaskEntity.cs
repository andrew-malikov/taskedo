namespace Taskedo.Tasks.Domain.CreateTask;

public class NewTaskEntity(string title,
                           string description,
                           DateTime dueDateAtUtc)
{
    public Guid TaskId { get; } = Guid.NewGuid();
    public string Title { get; } = title;
    public string Description { get; } = description;
    public DateTime DueDateAtUtc { get; } = dueDateAtUtc;
}
