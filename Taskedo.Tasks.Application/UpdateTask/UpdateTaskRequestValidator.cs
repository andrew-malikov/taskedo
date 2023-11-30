using FluentValidation;

namespace Taskedo.Tasks.Application.UpdateTask;

public class AddNewTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public AddNewTaskRequestValidator()
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
        RuleFor(t => t.Title).NotNull().NotEmpty().MaximumLength(255);
        RuleFor(t => t.Description).NotNull().MaximumLength(2047);
        RuleFor(t => t.DueDateAtUtc).NotNull();
        RuleFor(t => t.IsCompleted).NotNull();
    }
}
