using System.Text;
using FluentResults;

namespace Taskedo.Tasks.Application.QueryTasks;

public class QueryTasksRequest
{
    public string? PageToken { get; set; }
    public int PageSize { get; set; } = 10;
}

public static class AvailablePageSize
{
    public const int Min = 1;
    public const int Max = 100;
}

public static class PageTokenExtensions
{
    public static Result<DateTime> ParseDateTimeFromPageToken(this string pageToken)
    {
        try
        {
            var serializedDateTimeBytes = Convert.FromBase64String(pageToken);
            var serializedDateTime = Encoding.UTF8.GetString(serializedDateTimeBytes);
            var parsedDateTime = DateTime.Parse(serializedDateTime);
            return Result.Ok(parsedDateTime);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to parse DateTime from the page token").CausedBy(ex));
        }
    }

    public static Result<string> SerializeDateTimeAsPageToken(this DateTime dateTime)
    {
        try
        {
            var serializedDateTime = Encoding.UTF8.GetBytes(dateTime.ToString());
            return Convert.ToBase64String(serializedDateTime);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to serialize DateTime into Base64").CausedBy(ex));
        }
    }
}
