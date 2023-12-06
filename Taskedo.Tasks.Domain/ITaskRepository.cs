using FluentResults;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Domain;

public interface ITaskRepository
{
    Task<Result<Guid>> AddTaskAsync(NewTaskEntity newTask);

    /// <param name="pageSize">Number of tasks to return</param>
    /// <param name="pageToken">Begining of the page</param>
    /// <param name="search">Search by Title and Description</param>
    /// <param name="isCompleted">Filter by IsCompleted</param>
    /// <returns></returns>
    Task<Result<PagedTasks>> GetTasksAsync(
        int pageSize,
        IEnumerable<SortRule> sortRules,
        TaskPageToken? pageToken = null,
        string? search = null,
        bool? isCompleted = null);
    Task<Result> DeleteTaskAsync(Guid taskId);
    Task<Result> UpdateTaskAsync(TaskEntity updateTask);
    Task<Result<bool>> HasTaskAsync(Guid taskId);
    Task<Result<TaskEntity?>> GetTaskAsync(Guid taskId);
}
