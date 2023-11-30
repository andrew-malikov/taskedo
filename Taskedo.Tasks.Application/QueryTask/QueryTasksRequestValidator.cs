using FluentValidation;

namespace Taskedo.Tasks.Application.QueryTask;

public class QueryTaskRequestValidator : AbstractValidator<QueryTaskRequest>
{
    public QueryTaskRequestValidator()
    {
        RuleFor(q => q.TaskId)
            .NotNull()
            .Custom((id, context) =>
            {
                if (id == Guid.Empty)
                {
                    context.AddFailure("TaskId", "TaskId is required and shouldn't be empty UUID.");
                }
            });
    }
}
