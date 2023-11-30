using FluentValidation;
using MediatR;
using Taskedo.Tasks.Domain;
using Taskedo.Tasks.Domain.CreateTask;

public class AddNewTaskCommand : IRequest
{
    public required AddNewTaskRequest Payload { init; get; }
}

public class AddNewTaskHandler : IRequestHandler<AddNewTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IValidator<AddNewTaskRequest> _requestValidator;

    public AddNewTaskHandler(
        ITaskRepository taskRepository,
        IValidator<AddNewTaskRequest> requestValidator)
    {
        _taskRepository = taskRepository;
        _requestValidator = requestValidator;
    }

    // TODO: use special Error type
    public async Task Handle(AddNewTaskCommand command, CancellationToken cancellationToken)
    {
        // TODO: validate the command payload
        var validationResult = _requestValidator.Validate(command.Payload);

        NewTask? newTask;
        try
        {
            newTask = NewTask.From(command.Payload.Title, command.Payload.Description, command.Payload.DueDateAtUtc);
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