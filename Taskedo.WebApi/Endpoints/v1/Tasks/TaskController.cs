using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskedo.Tasks.Application.AddNewTask;
using Taskedo.Tasks.Application.DeleteTask;
using Taskedo.Tasks.Application.QueryTask;
using Taskedo.Tasks.Application.UpdateTask;

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
    /// Adds a new Task.
    /// </summary>
    /// <response code="200">Returns Task Id.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="500">Error while trying to add the Task. / Other errors</response>
    /// <param name="request">New Task</param>
    /// <returns>Added Task Id</returns>
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
    [HttpGet("/{taskId}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetTaskAsync([FromRoute] Guid taskId)
    {
        var request = new QueryTaskRequest { TaskId = taskId };
        var result = await _mediator.Send(new GetTaskQuery { Payload = request });
        return ToActionResult<TaskResponse>(result, _logger);
    }

    /// <summary>
    /// Deletes a Task
    /// </summary>
    /// <response code="200">Task is deleted.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="500">Error while trying to delete the Task. / Other errors</response>
    /// <param name="taskId">Task Id</param>
    /// <returns></returns>
    [HttpDelete("/{taskId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteTaskAsync([FromRoute] Guid taskId)
    {
        var request = new DeleteTaskRequest { TaskId = taskId };
        var result = await _mediator.Send(new DeleteTaskCommand { Payload = request });
        return ToActionResult<int>(result, _logger);
    }

    /// <summary>
    /// Updates a Task.
    /// </summary>
    /// <response code="200">Task is updated.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="500">Error while trying to update the Task. / Other errors</response>
    /// <param name="request">Updated Task</param>
    /// <returns>Updated task id</returns>
    [HttpPut]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateTaskAsync([FromBody] UpdateTaskRequest request)
    {
        var result = await _mediator.Send(new UpdateTaskCommand { Payload = request });
        return ToActionResult<uint>(result, _logger);
    }
}
