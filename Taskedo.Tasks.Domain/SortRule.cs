namespace Taskedo.Tasks.Domain;

public record SortRule(string Field, SortDirection Direction = SortDirection.Ascending);
