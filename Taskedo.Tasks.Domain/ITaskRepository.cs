using FluentResults;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Domain;

public interface ITaskRepository
{
    Task<Result<Guid>> AddTaskAsync(NewTaskEntity newTask);
    Task<Result<IEnumerable<SlimTaskEntity>>> GetAllTasksAsync();
    Task<Result> DeleteTaskAsync(Guid taskId);
    Task<Result> UpdateTaskAsync(TaskEntity updateTask);
    Task<Result<bool>> HasTaskAsync(Guid taskId);
    Task<Result<TaskEntity?>> GetTaskAsync(Guid taskId);
}
