using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using Shared.Persistence.Neo4j.Settings;

namespace Shared.Persistence.Neo4j;

public static class Neo4JExtensions
{
    public static IServiceCollection AddNeo4JDb(this IServiceCollection services,
        IConfiguration configuration)
    {
        var neo4JSettings = new Neo4JSettings();
        configuration.Bind("Neo4jSettings", neo4JSettings);

        services.AddSingleton(Options.Create(neo4JSettings));

        services.AddSingleton(GraphDatabase.Driver(
            neo4JSettings.Uri,
            AuthTokens.Basic(neo4JSettings.Username, neo4JSettings.Password)
        ));

        return services;
    }
}