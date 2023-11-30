using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Taskedo.WebApi.Endpoints.v1.Tasks;

[ApiController]
[Route("api/v1/task")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
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
    public async Task AddNewTaskAsync([FromBody] AddNewTaskRequest request)
    {
        await _mediator.Send(new AddNewTaskCommand { Payload = request });
    }
}
