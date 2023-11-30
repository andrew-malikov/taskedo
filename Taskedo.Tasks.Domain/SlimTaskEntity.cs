namespace Taskedo.Tasks.Domain;

public class SlimTaskEntity(Guid taskId,
                            string title,
                            DateTime dueDateAtUtc,
                            bool isCompleted)
{
    public Guid TaskId = taskId;
    public string Title = title;
    public DateTime DueDateAtUtc = dueDateAtUtc;
    public bool IsCompleted = isCompleted;
}
