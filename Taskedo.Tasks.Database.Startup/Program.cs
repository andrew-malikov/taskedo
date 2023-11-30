using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taskedo.Tasks.Database.EF;

namespace Taskedo.Tasks.Database.Startup;

public static class Program
{
    public static void Main(string[] args)
        => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args).ConfigureServices((services) =>
        {
            var connectionString = Environment.GetEnvironmentVariable("Db__MigrationConnectionString");
            services.AddDbContext<TasksContext>(builder => builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Taskedo.Tasks.Database.Changes")));
        });
}
