namespace Taskedo.Tasks.Application.AddNewTask;

public class AddNewTaskRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDateAtUtc { get; set; }
}

