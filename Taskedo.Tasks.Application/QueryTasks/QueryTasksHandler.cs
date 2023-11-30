using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application.QueryTasks;

public class QueryTasksRequest : IRequest<ICommandResult>
{
    public string PageToken { get; set; } = string.Empty;
    public string PageSize { get; set; } = string.Empty;
}

public class QueryTasksHandler : IRequestHandler<QueryTasksRequest, ICommandResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public QueryTasksHandler(
        ITaskRepository taskRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<ICommandResult> Handle(QueryTasksRequest query, CancellationToken cancellationToken)
    {
        var tasksResult = await _taskRepository.GetAllTasksAsync();
        if (tasksResult.IsFailed)
        {
            return new ICommandResult.InternalError
            {
                Errors = tasksResult.Errors
            };
        }

        IEnumerable<SlimTaskResponse> tasks;
        try
        {
            tasks = _mapper.Map<IEnumerable<SlimTaskResponse>>(tasksResult.Value);
        }
        catch (Exception ex)
        {
            return new ICommandResult.InternalError
            {
                Errors = Result.Fail(new Error("Failed to map Slim Task from Domain into Application reponse").CausedBy(ex)).Errors
            };
        }

        return new ICommandResult.Success<SlimTasksResponse>
        {
            Data = new SlimTasksResponse { Tasks = tasks }
        };
    }
}
