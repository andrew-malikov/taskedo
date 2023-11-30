using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Taskedo.Swagger;

public static class SwaggerConfig
{
    private static readonly string Name = "DELS API Version 1";
    private static readonly string Endpoint = "/swagger/v1/swagger.json";
    private static readonly string RoutePrefix = string.Empty;
    private static readonly DocExpansion DocExpansion = DocExpansion.None;
    private static readonly string Version = "v1";
    private static readonly string Title = $"Taskedo Tasks API";
    private static readonly string? DisplayVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    public static void SwaggerUIConfig(SwaggerUIOptions config)
    {
        config.SwaggerEndpoint(Endpoint, Name);
        config.RoutePrefix = RoutePrefix;
        config.DocExpansion(DocExpansion);
        config.DisplayRequestDuration();
        config.EnableFilter();
        config.EnableDeepLinking();
        config.ShowExtensions();
        config.DefaultModelRendering(ModelRendering.Example);
        config.DocumentTitle = $"{Title} API Docs";
    }

    public static void SwaggerGenConfig(SwaggerGenOptions config)
    {
        config.SwaggerDoc(Version, new OpenApiInfo
        {
            Version = Version,
            Title = Title,
        });

        config.ExampleFilters();

        config.CustomSchemaIds(x => x.FullName.Replace("+", "."));
        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        config.IncludeXmlComments(xmlPath, true);
    }

    private static OpenApiInfo GetOpenApiInfo()
    {
        return new OpenApiInfo
        {
            Version = DisplayVersion,
            Title = Title,
        };
    }
}