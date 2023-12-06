using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application.QueryTask;

public class GetTaskQuery : IRequest<ICommandResult>
{
    public QueryTaskRequest Payload;
}

public class QueryTaskHandler : IRequestHandler<GetTaskQuery, ICommandResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<QueryTaskRequest> _validator;

    public QueryTaskHandler(
        ITaskRepository taskRepository,
        IMapper mapper,
        IValidator<QueryTaskRequest> validator)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ICommandResult> Handle(GetTaskQuery query, CancellationToken cancellationToken)
    {
        var queryValidationResult = await _validator.ValidateAsync(query.Payload, cancellationToken);
        if (!queryValidationResult.IsValid)
        {
            return new ICommandResult.ValidationError
            {
                Errors = queryValidationResult.Errors
            };
        }

        var taskResult = await _taskRepository.GetTaskAsync(query.Payload.TaskId);
        if (taskResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = taskResult.Errors
            };
        }

        var task = taskResult.Value;
        if (task == null)
        {
            return new ICommandResult.NotFound { };
        }

        try
        {
            var responseTask = _mapper.Map<TaskResponse>(task);
            return new ICommandResult.Success<TaskResponse>
            {
                Data = responseTask
            };
        }
        catch (AutoMapperMappingException ex)
        {
            return new ICommandResult.InternalError
            {
                Errors = Result.Fail(new Error("Failed to map Task from Domain into Application reponse").CausedBy(ex)).Errors
            };
        }
    }
}
