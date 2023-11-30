using AutoMapper;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application.UpdateTask;

public class UpdateTaskCommand : IRequest<ICommandResult>
{
    public required UpdateTaskRequest Payload { init; get; }
}

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, ICommandResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IValidator<UpdateTaskRequest> _requestValidator;
    private readonly IMapper _mapper;

    public UpdateTaskHandler(
        ITaskRepository taskRepository,
        IValidator<UpdateTaskRequest> requestValidator,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _requestValidator = requestValidator;
        _mapper = mapper;
    }

    public async Task<ICommandResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
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
            return new ICommandResult.NotFound();
        }

        TaskEntity? updateTaskEntity = null;
        try
        {
            updateTaskEntity = _mapper.Map<UpdateTaskRequest, TaskEntity>(command.Payload);
        }
        catch (Exception ex)
        {
            return new ICommandResult.InternalError
            {
                Errors = new List<IError> { new Error("Failed to map Update Task Request into Domain Task.").CausedBy(ex) }
            };
        }

        var updateResult = await _taskRepository.UpdateTaskAsync(updateTaskEntity);
        if (updateResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = updateResult.Errors
            };
        }

        return new ICommandResult.Success();
    }
}
