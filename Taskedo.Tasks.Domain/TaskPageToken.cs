namespace Taskedo.Tasks.Domain;

public record TaskPageToken(Guid TaskId,
                            string Title,
                            DateTime DueDate,
                            bool IsCompleted,
                            DateTime CreatedDate)
{
    public static TaskPageToken Of(SlimTaskEntity task)
    {
        return new TaskPageToken(task.TaskId, task.Title, task.DueDateAtUtc, task.IsCompleted, task.CreatedAtUtc);
    }
}
