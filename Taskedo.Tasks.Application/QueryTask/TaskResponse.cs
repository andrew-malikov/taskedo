namespace Taskedo.Tasks.Application.QueryTask;

public class TaskResponse
{
    public Guid TaskId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public DateTime DueDateAtUtc { get; set; }
    public bool IsCompleted { get; set; }
}
