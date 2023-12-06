using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskedo.Tasks.Application;
using Taskedo.Tasks.Application.QueryTasks;
using Taskedo.Tasks.Domain;
using Taskedo.WebApi.Endpoints.Sorting;

namespace Taskedo.WebApi.Endpoints.v1.Tasks;

/// <summary>
/// Manages Task entity
/// </summary>
[ApiController]
[Route("api/v1/tasks")]
public class TasksController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(IMediator mediator, ILogger<TasksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Returs a paginates set of Tasks
    /// </summary>
    /// <param name="pageSize">Number of Tasks per page</param>
    /// <param name="pageToken">Page token</param>
    /// <param name="search">Filter by Task's Title or Description</param>
    /// <param name="isCompleted">Filter by Task's IsCompleted status</param>
    /// <param name="sortBy">A sort expression like "-title,dueDate". Supports next fields "title,dueDate,isCompleted,createdDate"</param>
    /// <response code="200">Returns Task's page.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="500">Error while trying fetch Tasks. / Other errors</response>
    /// <returns>Found set of Tasks according the page</returns>
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(SlimTasksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetTasksAsync(
        [FromQuery] int? pageSize,
        [FromQuery] string? pageToken,
        [FromQuery] string? search,
        [FromQuery] bool? isCompleted,
        [FromQuery] string? sortBy)
    {
        IEnumerable<SortRule> sortRules = new List<SortRule>();
        if (!string.IsNullOrEmpty(sortBy))
        {
            var rulesResult = sortBy.ParseSortRules();
            if (rulesResult.IsFailed)
            {
                return ToActionResult<SlimTasksResponse>(ICommandResult.ValidationError.Of(new ValidationFailure("sortBy", "SortBy is in invalid format.")), _logger);
            }
            sortRules = rulesResult.Value;
        }

        var queryRequest = new QueryTasksRequest
        {
            PageSize = pageSize ?? 10,
            PageToken = pageToken,
            Filter = new QueryTasksFilter
            {
                Search = search,
                IsCompleted = isCompleted
            },
            SortRules = sortRules
        };
        var result = await _mediator.Send(new GetTasksQuery { Payload = queryRequest });
        return ToActionResult<SlimTasksResponse>(result, _logger);
    }
}
