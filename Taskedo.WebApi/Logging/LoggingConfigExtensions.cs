using Serilog;
using Serilog.Events;

namespace Taskedo.WebApi.Logging;

public static class LoggingConfigExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        var logConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}:: {Message:lj}{NewLine}{Exception}");

        builder.Logging.ClearProviders().AddSerilog(logConfig.CreateLogger());

        return builder;
    }
}
