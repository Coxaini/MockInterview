using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Interviews.Application.Conferences.Abstractions;
using MockInterview.Interviews.Infrastructure.BackgroundServices;
using MockInterview.Interviews.Infrastructure.Repositories;

namespace MockInterview.Interviews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<IndexCreationService>();
        services.AddSingleton<IConferenceConnectionRepository, ConferenceConnectionRepository>();
        return services;
    }
}