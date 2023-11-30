using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Taskedo.Tasks.Database.EF;

namespace Taskedo.WebApi.Database;

internal static class DatabaseConfigExtensions
{
    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var dbOptionsSection = builder.Configuration.GetSection(DatabaseOptions.DatabaseSection);
        var dbOptions = new DatabaseOptions();
        dbOptionsSection.Bind(dbOptions);

        builder.Services
            .Configure<DatabaseOptions>(dbOptionsSection)
            .AddTransient<IValidator<DatabaseOptions>, DatabaseOptionsValidator>()
            .AddTransient<IStartupFilter, DatabaseOptionsStartupFilter>()
            .AddDbContext<TasksContext>(builder => builder.UseSqlServer(dbOptions.ConnectionString, b => b.MigrationsAssembly("Taskedo.Tasks.Database.Changes")));

        return builder;
    }
}