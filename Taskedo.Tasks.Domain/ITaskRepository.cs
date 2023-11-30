using FluentResults;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Domain;

public interface ITaskRepository
{
    Task<Result<Guid>> AddTaskAsync(NewTaskEntity newTask);

    /// <param name="pageSize">Number of tasks to return</param>
    /// <param name="pageToken">Begining of the page</param>
    /// <returns></returns>
    Task<Result<IEnumerable<SlimTaskEntity>>> GetAllTasksAsync(int pageSize, DateTime? fromCreatedAtUtc = null);
    Task<Result> DeleteTaskAsync(Guid taskId);
    Task<Result> UpdateTaskAsync(TaskEntity updateTask);
    Task<Result<bool>> HasTaskAsync(Guid taskId);
    Task<Result<TaskEntity?>> GetTaskAsync(Guid taskId);
}
