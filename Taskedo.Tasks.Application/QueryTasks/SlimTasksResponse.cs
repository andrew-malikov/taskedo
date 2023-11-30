namespace Taskedo.Tasks.Application.QueryTasks;

public class SlimTaskResponse
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public DateTime DueDateAtUtc { get; set; }
    public bool IsCompleted { get; set; }
}

public class SlimTasksResponse
{
    public IEnumerable<SlimTaskResponse> Tasks { get; set; }
    public string NextPageToken { get; set; } = string.Empty;
}
