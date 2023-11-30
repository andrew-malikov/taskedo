namespace Taskedo.WebApi.Database;

public class DatabaseOptions
{
    public const string DatabaseSection = "Db";

    public string ConnectionString { get; set; } = string.Empty;
}