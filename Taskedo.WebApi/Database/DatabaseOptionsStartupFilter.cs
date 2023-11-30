using FluentValidation;
using Microsoft.Extensions.Options;

namespace Taskedo.WebApi.Database;

public class DatabaseOptionsStartupFilter : IStartupFilter
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly IValidator<DatabaseOptions> _databaseOptionsValidator;

    public DatabaseOptionsStartupFilter(
        IOptionsMonitor<DatabaseOptions> databaseConfiguration,
        IValidator<DatabaseOptions> databaseConfigurationValidator)
    {
        _databaseOptions = databaseConfiguration.CurrentValue;
        _databaseOptionsValidator = databaseConfigurationValidator;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        var resultValidation = _databaseOptionsValidator.Validate(_databaseOptions);
        if (!resultValidation.IsValid)
        {
            throw new ArgumentException(resultValidation.ToString());
        }

        return next;
    }
}