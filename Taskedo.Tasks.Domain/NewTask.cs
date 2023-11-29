namespace Taskedo.Tasks.Domain.CreateTask;

public class NewTask
{
    public readonly Guid TaskId;
    public readonly string Title;
    public readonly string Description;
    public readonly DateTime DueDateAtUtc;

    private NewTask(Guid taskId,
                    string title,
                    string description,
                    DateTime dueDateAtUtc)
    {
        TaskId = taskId;
        Title = title;
        Description = description;
        DueDateAtUtc = dueDateAtUtc;
    }

    /// <summary>
    /// Validates the args and throws an ArguementException in case of errors
    /// </summary>
    /// <param name="title"></param>
    /// <param name="Description"></param>
    /// <param name="dueDateAtUtc"></param>
    /// <returns></returns>
    public static NewTask From(string title, string description, DateTime dueDateAtUtc)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title of new task can not be empty.");
        }

        return new NewTask(Guid.NewGuid(), title, description, dueDateAtUtc);
    }
}
