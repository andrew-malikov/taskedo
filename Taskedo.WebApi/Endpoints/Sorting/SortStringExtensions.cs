using FluentResults;
using Taskedo.Tasks.Domain;

namespace Taskedo.WebApi.Endpoints.Sorting;

public static class SortStringExtensions
{
    public static Result<IEnumerable<SortRule>> ParseSortRules(this string sortExpression)
    {
        try
        {
            IEnumerable<SortRule> sortRules = sortExpression
                .Split(',')
                .Select(x =>
                {
                    if (x.StartsWith('-'))
                    {
                        return new SortRule(x.TrimStart('-'), SortDirection.Descending);
                    }
                    return new SortRule(x, SortDirection.Ascending);
                })
                .Where(rule => !string.IsNullOrWhiteSpace(rule.Field))
                .ToList();
            return Result.Ok(sortRules);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to parse sort expression into a set of rules.").CausedBy(ex));
        }
    }
}