namespace Taskedo.Tasks.Domain;

public class TaskEntity(Guid taskId,
                        string title,
                        string description,
                        DateTime dueDateAtUtc,
                        bool isCompleted)
{
    public Guid TaskId { get; } = taskId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public DateTime DueDateAtUtc { get; } = dueDateAtUtc;
    public bool IsCompleted { get; } = isCompleted;
}
