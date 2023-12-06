using System.Linq.Expressions;
using Taskedo.Tasks.Domain;

namespace Taskedo.Tasks.Database.Context;

public static class ContextExtensions
{
    public static IOrderedQueryable<TModel> OrderBySortRule<TModel, TKey>(this IQueryable<TModel> query, Expression<Func<TModel, TKey>> selector, SortDirection direction)
    {
        return direction switch
        {
            SortDirection.Ascending => query.OrderBy(selector),
            SortDirection.Descending => query.OrderByDescending(selector),
            _ => throw new NotImplementedException(),
        };
    }

    public static IOrderedQueryable<TModel> OrderBySortRule<TModel, TKey>(this IOrderedQueryable<TModel> query, Expression<Func<TModel, TKey>> selector, SortDirection direction)
    {
        return direction switch
        {
            SortDirection.Ascending => query.OrderBy(selector),
            SortDirection.Descending => query.OrderByDescending(selector),
            _ => throw new NotImplementedException(),
        };
    }
}
