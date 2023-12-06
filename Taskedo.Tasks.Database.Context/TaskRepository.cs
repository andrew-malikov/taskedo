using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Taskedo.Tasks.Domain;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Database.Context;

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
        catch (AutoMapperMappingException ex)
        {
            return Result.Fail(new Error("Failed to map a new Task to DB Entity.").CausedBy(ex));
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

    public async Task<Result<PagedTasks>> GetTasksAsync(
        int pageSize,
        IEnumerable<SortRule> sortRules,
        TaskPageToken? pageToken = null,
        string? search = null,
        bool? isCompleted = null)
    {
        try
        {
            var tasksQuery = _context.Tasks
                .AsNoTracking();

            if (search != null)
            {
                tasksQuery = tasksQuery.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            if (isCompleted != null && isCompleted.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.IsCompleted == isCompleted);
            }

            if (sortRules.Any())
            {
                if (pageToken != null)
                {
                    foreach (var sortRule in sortRules)
                    {
                        tasksQuery = sortRule.Field switch
                        {
                            "dueDate" => tasksQuery.Where(t => t.DueDateAtUtc < pageToken.DueDate),
                            "createdDate" => tasksQuery.Where(t => t.CreatedAtUtc < pageToken.CreatedDate),
                            "title" => tasksQuery.Where(t => t.Title.CompareTo(pageToken.Title) < 0),
                            "isCompleted" => tasksQuery.Where(t => t.IsCompleted.CompareTo(pageToken.IsCompleted) < 0),
                            _ => tasksQuery
                        };
                    }
                }

                var firstSortRule = sortRules.First();
                var orderTasksQueury = firstSortRule.Field switch
                {
                    "dueDate" => tasksQuery.OrderBySortRule(t => t.DueDateAtUtc, firstSortRule.Direction),
                    "createdDate" => tasksQuery.OrderBySortRule(t => t.CreatedAtUtc, firstSortRule.Direction),
                    "title" => tasksQuery.OrderBySortRule(t => t.Title, firstSortRule.Direction),
                    "isCompleted" => tasksQuery.OrderBySortRule(t => t.IsCompleted, firstSortRule.Direction),
                };

                orderTasksQueury = sortRules.Skip(1).Aggregate(orderTasksQueury, (current, sortRule) =>
                {
                    return sortRule.Field switch
                    {
                        "dueDate" => current.OrderBySortRule(t => t.DueDateAtUtc, sortRule.Direction),
                        "createdDate" => current.OrderBySortRule(t => t.CreatedAtUtc, sortRule.Direction),
                        "title" => current.OrderBySortRule(t => t.Title, sortRule.Direction),
                        "isCompleted" => current.OrderBySortRule(t => t.IsCompleted, sortRule.Direction),
                        _ => current
                    };
                });
                tasksQuery = orderTasksQueury;
            }
            else
            {
                if (pageToken != null)
                {
                    tasksQuery = tasksQuery
                        .Where(t => t.CreatedAtUtc <= pageToken.CreatedDate);
                }

                tasksQuery = tasksQuery
                    .OrderByDescending(t => t.CreatedAtUtc)
                    .ThenBy(t => t.TaskId);
            }

            var tasks = await tasksQuery
                .Select(t => new SlimTaskEntity(t.TaskId, t.Title, t.DueDateAtUtc, t.IsCompleted, t.CreatedAtUtc))
                .Take(pageSize)
                .ToListAsync();

            var lastTask = tasks.LastOrDefault();
            if (lastTask == null)
            {
                return new PagedTasks(tasks, null).ToResult();
            }
            return new PagedTasks(tasks, TaskPageToken.Of(lastTask)).ToResult();
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
        catch (AutoMapperMappingException ex)
        {
            return Result.Fail(new Error("Failed to map a Task to Domain Entity.").CausedBy(ex));
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