namespace Taskedo.Tasks.Application.UpdateTask;

public class UpdateTaskRequest
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDateAtUtc { get; set; }
    public bool IsCompleted { get; set; }
}
