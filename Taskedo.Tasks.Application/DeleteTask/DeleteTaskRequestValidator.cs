using FluentValidation;

namespace Taskedo.Tasks.Application.DeleteTask;

public class DeleteTaskRequestValidator : AbstractValidator<DeleteTaskRequest>
{
    public DeleteTaskRequestValidator()
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
