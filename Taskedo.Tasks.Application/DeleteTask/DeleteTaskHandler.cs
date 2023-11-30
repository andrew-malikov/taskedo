using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application.DeleteTask;

public class DeleteTaskCommand : IRequest<ICommandResult>
{
    public required DeleteTaskRequest Payload { init; get; }
}

public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, ICommandResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IValidator<DeleteTaskRequest> _requestValidator;

    public DeleteTaskHandler(
        ITaskRepository taskRepository,
        IValidator<DeleteTaskRequest> requestValidator)
    {
        _taskRepository = taskRepository;
        _requestValidator = requestValidator;
    }

    public async Task<ICommandResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _requestValidator.Validate(command.Payload);
        if (!validationResult.IsValid)
        {
            return new ICommandResult.ValidationError
            {
                Errors = validationResult.Errors
            };
        }

        var hasTaskResult = await _taskRepository.HasTaskAsync(command.Payload.TaskId);
        if (hasTaskResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = hasTaskResult.Errors
            };
        }

        if (!hasTaskResult.Value)
        {
            return new ICommandResult.Success();
        }

        var deleteResult = await _taskRepository.DeleteTaskAsync(command.Payload.TaskId);
        if (deleteResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = deleteResult.Errors
            };
        }

        return new ICommandResult.Success();
    }
}
