namespace Taskedo.Tasks.Domain;

public class TaskEntity
{
    public Guid TaskkId;
    public string Title;
    public string Description;
    public DateTime DueDateAtUtc;
    public bool IsCompleted;

    public TaskEntity(Guid taskId,
                      string title,
                      string description,
                      DateTime dueDateAtUtc,
                      bool isCompleted)
    {
        TaskkId = taskId;
        Title = title;
        Description = description;
        DueDateAtUtc = dueDateAtUtc;
        IsCompleted = isCompleted;
    }
}
