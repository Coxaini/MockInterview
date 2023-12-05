using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Matchmaking.Application;
using MockInterview.Matchmaking.Infrastructure;
using Shared.Core.Mappings;

namespace MockInterview.Matchmaking.API;

public static class MatchmakingModule
{
    public static IServiceCollection AddMatchmakingModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ScanMappings(typeof(Application.DependencyInjection).Assembly);

        services
            .AddApplication()
            .AddInfrastructure(configuration);

        return services;
    }
}