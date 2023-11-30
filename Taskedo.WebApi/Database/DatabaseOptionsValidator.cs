using FluentValidation;

namespace Taskedo.WebApi.Database;

internal class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(d => d.ConnectionString).NotEmpty();
    }
}