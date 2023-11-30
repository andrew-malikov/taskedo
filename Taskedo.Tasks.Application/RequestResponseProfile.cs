using AutoMapper;
using Taskedo.Tasks.Application.QueryTask;
using Taskedo.Tasks.Application.QueryTasks;
using Taskedo.Tasks.Application.UpdateTask;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application;

public class RequestResponseProfile : Profile
{
    public RequestResponseProfile()
    {
        CreateMap<TaskEntity, TaskResponse>();
        CreateMap<SlimTaskEntity, SlimTaskResponse>();
        CreateMap<UpdateTaskRequest, TaskEntity>();
    }
}
