namespace Taskedo.WebApi.Database;

internal class DatabaseOptions
{
    public const string DatabaseSection = "Db";

    public string ConnectionString { get; set; } = string.Empty;
}