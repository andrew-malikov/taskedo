using FluentValidation;

namespace Taskedo.Tasks.Application.QueryTasks;

public class QueryTasksRequestValidator : AbstractValidator<QueryTasksRequest>
{
    public QueryTasksRequestValidator()
    {
        RuleFor(q => q.PageSize)
            .NotNull()
            .GreaterThanOrEqualTo(AvailablePageSize.Min)
            .LessThanOrEqualTo(AvailablePageSize.Max);

        RuleFor(q => q.Filter)
            .NotNull();

        RuleFor(q => q.Filter.Search)
            .MaximumLength(1027);

        RuleFor(q => q.SortRules)
            .NotNull();
    }
}
