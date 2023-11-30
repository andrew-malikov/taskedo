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
            var task = await _context.Tasks.FirstAsync(t => t.TaskId == taskId);
            task.IsDeleted = true;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to delete a Task from DB.").CausedBy(ex));
        }
    }

    public async Task<Result<IEnumerable<SlimTaskEntity>>> GetAllTasksAsync()
    {
        try
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .Select(t => new SlimTaskEntity(t.TaskId, t.Title, t.DueDateAtUtc, t.IsCompleted))
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
            _context.Attach(updateTask);
            _context.Entry(updateTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to update a Task in DB.").CausedBy(ex));
        }
    }
}