using FluentValidation;

namespace Taskedo.Tasks.Application.AddNewTask;

public class AddNewTaskRequestValidator : AbstractValidator<AddNewTaskRequest>
{
    public AddNewTaskRequestValidator()
    {
        RuleFor(t => t.Title).NotNull().NotEmpty();
        RuleFor(t => t.Description).NotNull();
        RuleFor(t => t.DueDateAtUtc).NotNull();
    }
}
