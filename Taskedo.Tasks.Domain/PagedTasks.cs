namespace Taskedo.Tasks.Domain;

public record PagedTasks(
    IEnumerable<SlimTaskEntity> Tasks,
    TaskPageToken? PageToken
);
