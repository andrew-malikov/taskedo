using UUIDNext;

namespace Taskedo.Tasks.Domain.CreateTask;

public class NewTaskEntity(string title,
                           string description,
                           DateTime dueDateAtUtc)
{
    public Guid TaskId { get; } = Uuid.NewSequential();
    public string Title { get; } = title;
    public string Description { get; } = description;
    public DateTime DueDateAtUtc { get; } = dueDateAtUtc.ToUniversalTime();
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}
