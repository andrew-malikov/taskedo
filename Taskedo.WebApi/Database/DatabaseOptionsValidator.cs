using FluentValidation;

namespace Taskedo.WebApi.Database;

public class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(d => d.ConnectionString).NotEmpty();
    }
}