using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Domain;

public interface ITaskRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Guid> AddTaskAsync(NewTask newTask);
}
