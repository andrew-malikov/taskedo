using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Taskedo.Tasks.Domain;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Application.AddNewTask;

public class AddNewTaskCommand : IRequest<ICommandResult>
{
    public required AddNewTaskRequest Payload { init; get; }
}

public class AddNewTaskHandler : IRequestHandler<AddNewTaskCommand, ICommandResult>
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

    public async Task<ICommandResult> Handle(AddNewTaskCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _requestValidator.Validate(command.Payload);
        if (!validationResult.IsValid)
        {
            return new ICommandResult.ValidationError
            {
                Errors = validationResult.Errors
            };
        }

        var newTask = new NewTaskEntity(command.Payload.Title, command.Payload.Description, command.Payload.DueDateAtUtc);

        var saveResult = await _taskRepository.AddTaskAsync(newTask);
        if (saveResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = saveResult.Errors
            };
        }

        return new ICommandResult.Success<Guid>
        {
            Data = saveResult.Value
        };
    }
}
