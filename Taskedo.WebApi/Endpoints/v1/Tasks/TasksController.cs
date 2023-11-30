using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskedo.Tasks.Application.QueryTasks;

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
    /// <response code="200">Returns Task's page.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="500">Error while trying fetch Tasks. / Other errors</response>
    /// <returns>Found set of Tasks according the page</returns>
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetTasksAsync([FromQuery] int? pageSize, [FromQuery] string? pageToken)
    {
        var queryRequest = new QueryTasksRequest { PageSize = pageSize ?? 10, PageToken = pageToken };
        var result = await _mediator.Send(new GetTasksQuery { Payload = queryRequest });
        return ToActionResult<SlimTasksResponse>(result, _logger);
    }
}
