using AutoMapper;
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

    public async Task<Guid> AddTaskAsync(NewTask newTask)
    {
        var newTaskDbEntity = _mapper.Map<TaskDbEntity>(newTask);
        await _context.Tasks.AddAsync(newTaskDbEntity);
        await _context.SaveChangesAsync();
        return newTaskDbEntity.TaskId;
    }
}