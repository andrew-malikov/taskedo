using AutoMapper;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application.QueryTasks;

public class GetTasksQuery : IRequest<ICommandResult>
{
    public QueryTasksRequest Payload;
}

public class QueryTasksHandler : IRequestHandler<GetTasksQuery, ICommandResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<QueryTasksRequest> _validator;

    public QueryTasksHandler(
        ITaskRepository taskRepository,
        IMapper mapper,
        IValidator<QueryTasksRequest> validator)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ICommandResult> Handle(GetTasksQuery query, CancellationToken cancellationToken)
    {
        var queryValidationResult = await _validator.ValidateAsync(query.Payload, cancellationToken);
        if (!queryValidationResult.IsValid)
        {
            return new ICommandResult.ValidationError
            {
                Errors = queryValidationResult.Errors
            };
        }

        TaskPageToken? pageToken = null;
        if (query.Payload.PageToken != null)
        {
            var pageTokenResult = query.Payload.PageToken.ParseTaskPageToken();
            if (pageTokenResult.IsFailed)
            {
                return new ICommandResult.ValidationError
                {
                    Errors = new List<ValidationFailure> { new("pageToken", "Page Token is invalid.") }
                };
            }

            pageToken = pageTokenResult.Value;
        }

        var pagedTasksResult = await _taskRepository.GetTasksAsync(
            query.Payload.PageSize,
            query.Payload.SortRules,
            pageToken,
            query.Payload.Filter.Search,
            query.Payload.Filter.IsCompleted);
        if (pagedTasksResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = pagedTasksResult.Errors
            };
        }

        var pagedTasks = pagedTasksResult.Value;
        string? nextPageToken = null;
        if (pagedTasks.PageToken != null)
        {
            var nextPageTokenResult = pagedTasks.PageToken.SerializeTaskPageToken();
            if (nextPageTokenResult.IsFailed)
            {
                return new ICommandResult.InternalError
                {
                    Errors = nextPageTokenResult.Errors
                };
            }
            nextPageToken = nextPageTokenResult.Value;
        }

        IEnumerable<SlimTaskResponse> responseTasks;
        try
        {
            responseTasks = _mapper.Map<IEnumerable<SlimTaskResponse>>(pagedTasks.Tasks);
        }
        catch (AutoMapperMappingException ex)
        {
            return new ICommandResult.InternalError
            {
                Errors = Result.Fail(new Error("Failed to map Slim Task from Domain into Application reponse").CausedBy(ex)).Errors
            };
        }

        return new ICommandResult.Success<SlimTasksResponse>
        {
            Data = new SlimTasksResponse { Tasks = responseTasks, NextPageToken = nextPageToken }
        };
    }
}
