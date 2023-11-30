using AutoMapper;
using Taskedo.Tasks.Application.QueryTasks;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application;

public class RequestResponseProfile : Profile
{
    public RequestResponseProfile()
    {
        CreateMap<SlimTaskEntity, SlimTaskResponse>();
    }
}
