using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Taskedo.Tasks.Domain;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Database.EF;

public class TaskRepository : ITaskRepository
{
    private readonly TasksContext _context;
    private readonly IMapper _mapper;

    public TaskRepository(TasksContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> AddTaskAsync(NewTaskEntity newTask)
    {
        try
        {
            var newTaskDbEntity = _mapper.Map<TaskDbEntity>(newTask);
            await _context.Tasks.AddAsync(newTaskDbEntity);
            await _context.SaveChangesAsync();
            return Result.Ok(newTaskDbEntity.TaskId);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to insert a new Task into DB.").CausedBy(ex));
        }
    }

    public async Task<Result> DeleteTaskAsync(Guid taskId)
    {
        try
        {
            await _context.Tasks
                .Where(t => t.TaskId == taskId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.IsDeleted, true));
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to delete a Task from DB.").CausedBy(ex));
        }
    }

    public async Task<Result<IEnumerable<SlimTaskEntity>>> GetAllTasksAsync(int pageSize, DateTime? pageToken = null)
    {
        try
        {
            var tasksQuery = _context.Tasks
                .AsNoTracking();

            if (pageToken != null && pageToken.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.CreatedAtUtc <= pageToken);
            }

            var tasks = await tasksQuery
                .OrderByDescending(t => t.CreatedAtUtc)
                .ThenBy(t => t.TaskId)
                .Select(t => new SlimTaskEntity(t.TaskId, t.Title, t.DueDateAtUtc, t.IsCompleted, t.CreatedAtUtc))
                .Take(pageSize)
                .ToListAsync();

            return Result.Ok(tasks as IEnumerable<SlimTaskEntity>);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to get all Tasks from DB.").CausedBy(ex));
        }
    }

    public async Task<Result<TaskEntity?>> GetTaskAsync(Guid taskId)
    {
        try
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
            if (task != null)
            {
                return Result.Ok(_mapper.Map<TaskEntity?>(task));
            }

            return Result.Ok<TaskEntity?>(null);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to get a Task from DB.").CausedBy(ex));
        }
    }

    public async Task<Result<bool>> HasTaskAsync(Guid taskId)
    {
        try
        {
            var hasTask = await _context.Tasks.AnyAsync(t => t.TaskId == taskId);
            return Result.Ok(hasTask);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to check if Task exists in DB.").CausedBy(ex));
        }
    }

    public async Task<Result> UpdateTaskAsync(TaskEntity updateTask)
    {
        try
        {
            await _context.Tasks
                .Where(t => t.TaskId == updateTask.TaskId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.Title, updateTask.Title)
                    .SetProperty(t => t.Description, updateTask.Description)
                    .SetProperty(t => t.DueDateAtUtc, updateTask.DueDateAtUtc)
                    .SetProperty(t => t.IsCompleted, updateTask.IsCompleted));
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to update a Task in DB.").CausedBy(ex));
        }
    }
}