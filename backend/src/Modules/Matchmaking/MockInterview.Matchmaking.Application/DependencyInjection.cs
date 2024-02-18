using Microsoft.Extensions.DependencyInjection;
using MockInterview.Matchmaking.Application.Matching.Services;

namespace MockInterview.Matchmaking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IMatchingService, MatchingService>();

        return services;
    }
}