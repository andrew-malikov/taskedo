using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;
using Taskedo.Swagger;
using Taskedo.Tasks.Application.AddNewTask;
using Taskedo.Tasks.Database.EF;
using Taskedo.WebApi.Database;
using Taskedo.WebApi.Logging;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.ConfigureLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfig.SwaggerGenConfig)
        .AddFluentValidationClientsideAdapters()
        .AddFluentValidationRulesToSwagger()
        .AddSwaggerExamplesFromAssemblyOf<Program>();
builder.ConfigureDatabase();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(AddNewTaskHandler).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<AddNewTaskRequestValidator>();
builder.Services.AddAutoMapper(typeof(DbEntitiesProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    using var appContext = scope.ServiceProvider.GetRequiredService<TasksContext>();

    try
    {
        await appContext.Database.MigrateAsync();
    }
    catch (Exception)
    {
        throw;
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
