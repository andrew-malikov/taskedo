using MediatR;
using Taskedo.Tasks.Domain;
using Taskedo.Tasks.Domain.CreateTask;

public class AddNewTaskHandler : IRequestHandler<AddNewTaskRequest>
{
    private readonly ITaskRepository _taskRepository;

    public AddNewTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    // TODO: use special Error type
    public async Task Handle(AddNewTaskRequest request, CancellationToken cancellationToken)
    {
        NewTask? newTask;
        try
        {
            newTask = NewTask.From(request.Title, request.Description, request.DueDateAtUtc);
        }
        catch (ArgumentException)
        {
            // TODO: return ValidationError
            return;
        }

        try
        {

            await _taskRepository.AddTaskAsync(newTask);
        }
        catch (Exception)
        {
            // TODO: return InternalError
            return;
        }

        return;
    }
}