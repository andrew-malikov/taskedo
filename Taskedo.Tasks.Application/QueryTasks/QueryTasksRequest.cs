using System.Text;
using FluentResults;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Application.QueryTasks;

public class QueryTasksRequest
{
    public string? PageToken { get; set; }
    public int PageSize { get; set; } = 10;
    public QueryTasksFilter Filter { get; set; }
    public IEnumerable<SortRule> SortRules;
}

public class QueryTasksFilter
{
    public string? Search { get; set; }
    public bool? IsCompleted { get; set; }
}

public static class AvailablePageSize
{
    public const int Min = 1;
    public const int Max = 100;
}

public static class PageTokenExtensions
{
    private const string FIELD_SEPARATOR = "::";
    public static Result<TaskPageToken> ParseTaskPageToken(this string pageToken)
    {
        try
        {
            var serializedTokenBytes = Convert.FromBase64String(pageToken);
            var serializedToken = Encoding.UTF8.GetString(serializedTokenBytes);
            var fields = serializedToken.Split(FIELD_SEPARATOR, 5);
            if (fields.Length != 5)
            {
                return Result.Fail(new Error("Failed to parse page token"));
            }
            var createdAtUtc = DateTime.Parse(fields[0]);
            var taskId = Guid.Parse(fields[1]);
            var dueDateAtUtc = DateTime.Parse(fields[2]);
            var isCompleted = bool.Parse(fields[3]);
            var title = fields[4];
            return new TaskPageToken(taskId, title, dueDateAtUtc, isCompleted, createdAtUtc).ToResult();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to parse TaskPageToken from string").CausedBy(ex));
        }
    }

    public static Result<string> SerializeTaskPageToken(this TaskPageToken pageToken)
    {
        try
        {
            var serializedToken = $"{pageToken.CreatedDate}::{pageToken.TaskId}::{pageToken.DueDate}::{pageToken.IsCompleted}::{pageToken.Title}";
            var serializedDateTime = Encoding.UTF8.GetBytes(serializedToken);
            return Convert.ToBase64String(serializedDateTime);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to serialize TaskPageToken into Base64").CausedBy(ex));
        }
    }
}
