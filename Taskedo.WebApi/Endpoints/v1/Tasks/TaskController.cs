using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskedo.Tasks.Application.AddNewTask;
using Taskedo.Tasks.Application.QueryTask;

namespace Taskedo.WebApi.Endpoints.v1.Tasks;

/// <summary>
/// Manages Task entity
/// </summary>
[ApiController]
[Route("api/v1/task")]
public class TaskController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskController> _logger;

    public TaskController(IMediator mediator, ILogger<TaskController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Adds a new task.
    /// </summary>
    /// <response code="200">Returns task id.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="500">Error while trying to add the task. / Other errors</response>
    /// <param name="request">New task</param>
    /// <returns>Added task id</returns>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddNewTaskAsync([FromBody] AddNewTaskRequest request)
    {
        var result = await _mediator.Send(new AddNewTaskCommand { Payload = request });
        return ToActionResult<Guid>(result, _logger);
    }

    /// <summary>
    /// Return a Task
    /// </summary>
    /// <response code="200">Returns Task.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="404">Task not found.</response>
    /// <response code="500">Error while trying to fetch the Task. / Other errors</response>
    /// <param name="taskId">Task Id</param>
    /// <returns>Found Task</returns>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetTaskAsync([FromQuery] Guid taskId)
    {
        var request = new QueryTaskRequest { TaskId = taskId };
        var result = await _mediator.Send(new GetTaskQuery { Payload = request });
        return ToActionResult<TaskResponse>(result, _logger);
    }
}
