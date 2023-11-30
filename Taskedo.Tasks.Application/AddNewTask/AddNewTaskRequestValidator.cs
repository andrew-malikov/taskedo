using FluentValidation;

namespace Taskedo.Tasks.Application.AddNewTask;

public class AddNewTaskRequestValidator : AbstractValidator<AddNewTaskRequest>
{
    public AddNewTaskRequestValidator()
    {
        RuleFor(t => t.Title).NotNull().NotEmpty().MaximumLength(255);
        RuleFor(t => t.Description).NotNull().MaximumLength(2047);
        RuleFor(t => t.DueDateAtUtc).NotNull();
    }
}
