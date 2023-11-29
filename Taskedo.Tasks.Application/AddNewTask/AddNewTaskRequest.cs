using MediatR;

public class AddNewTaskRequest : IRequest
{
    public string Title { init; get; }
    public string Description { init; get; }
    public DateTime DueDateAtUtc { init; get; }
}

