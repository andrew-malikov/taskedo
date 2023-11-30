using AutoMapper;
using Taskedo.Tasks.Domain.CreateTask;

namespace Taskedo.Tasks.Database.EF;

public class DbEntitiesProfile : Profile
{
    public DbEntitiesProfile()
    {
        CreateMap<NewTaskEntity, TaskDbEntity>()
            .ForMember(dest => dest.IsCompleted, (opt) => opt.MapFrom(_ => false));
        CreateMap<TaskDbEntity, Task>();
    }
}
