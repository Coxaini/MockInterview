namespace Shared.Persistence.Neo4j.Settings;

public class Neo4JSettings
{
    public string Uri { get; init; } = null!;
    public string Database { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}