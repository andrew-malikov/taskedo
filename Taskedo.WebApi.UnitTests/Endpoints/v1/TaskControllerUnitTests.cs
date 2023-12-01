using Taskedo.WebApi.Endpoints.v1.Tasks;
using MediatR;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Taskedo.Tasks.Application.QueryTask;
using Taskedo.Tasks.Application;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace Taskedo.WebApi.UnitTests.Endpoints.v1;

public class TaskControllerUnitTests
{
    private readonly TaskController _controller;
    private readonly IMediator _mediatorMock;
    private readonly ILogger<TaskController> _loggerMock;

    public TaskControllerUnitTests()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _loggerMock = Substitute.For<ILogger<TaskController>>();
        _controller = new TaskController(_mediatorMock, _loggerMock);
    }

    [Fact]
    public async Task Given_QueryTaskRequestAndValidTaskId_When_GetTaskAsync_Then_ReturnFoundTaskResponse()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var foundTaskResponse = new TaskResponse
        {
            TaskId = taskId,
            Title = "Get Outside",
            Description = "Go outside and enjoy the sun",
            IsCompleted = false,
            DueDateAtUtc = DateTime.UtcNow.AddDays(1),
        };
        ICommandResult queryResult = new ICommandResult.Success<TaskResponse> { Data = foundTaskResponse };

        _mediatorMock
            .Send(Arg.Is<GetTaskQuery>(x => x.Payload.TaskId == taskId))
            .Returns(Task.FromResult(queryResult));

        // Act
        var actualResult = await _controller.GetTaskAsync(taskId);

        // Assert
        actualResult.Should().BeOfType<OkObjectResult>();
        var okResult = actualResult as OkObjectResult;
        okResult!.Value.Should().BeOfType<TaskResponse>();
        var taskResponse = okResult.Value as TaskResponse;
        taskResponse!.TaskId.Should().Be(foundTaskResponse.TaskId);
        taskResponse.Title.Should().Be(foundTaskResponse.Title);
        taskResponse.Description.Should().Be(foundTaskResponse.Description);
        taskResponse.IsCompleted.Should().Be(foundTaskResponse.IsCompleted);
        taskResponse.DueDateAtUtc.Should().Be(foundTaskResponse.DueDateAtUtc);
    }

    [Fact]
    public async Task Given_QueryTaskRequestAndNonExistingTaskId_When_GetTaskAsync_Then_ReturnNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        ICommandResult queryResult = new ICommandResult.NotFound { };

        _mediatorMock
            .Send(Arg.Is<GetTaskQuery>(x => x.Payload.TaskId == taskId))
            .Returns(Task.FromResult(queryResult));

        // Act
        var actualResult = await _controller.GetTaskAsync(taskId);

        // Assert
        actualResult.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Given_QueryTaskRequestAndInternalError_When_GetTaskAsync_Then_ReturnInternalError()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        ICommandResult queryResult = ICommandResult.InternalError.Of("Well smth's happend and everything is crashed down.");

        _mediatorMock
            .Send(Arg.Is<GetTaskQuery>(x => x.Payload.TaskId == taskId))
            .Returns(Task.FromResult(queryResult));

        // Act
        var actualResult = await _controller.GetTaskAsync(taskId);

        // Assert
        actualResult.Should().BeOfType<StatusCodeResult>();
        var statusCodeResult = actualResult as StatusCodeResult;
        statusCodeResult!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task Given_QueryTaskRequestAndInvalidTaskId_When_GetTaskAsync_Then_ReturnValidationError()
    {
        // Arrange
        var taskId = Guid.Empty;
        ICommandResult queryResult = ICommandResult.ValidationError.Of(new ValidationFailure("taskId", "TaskId should be a valid UUID."));

        _mediatorMock
            .Send(Arg.Is<GetTaskQuery>(x => x.Payload.TaskId == taskId))
            .Returns(Task.FromResult(queryResult));

        // Act
        var actualResult = await _controller.GetTaskAsync(taskId);

        // Assert
        actualResult.Should().BeOfType<BadRequestObjectResult>();
        var statusCodeResult = actualResult as BadRequestObjectResult;
        statusCodeResult!.StatusCode.Should().Be(400);
    }
}
