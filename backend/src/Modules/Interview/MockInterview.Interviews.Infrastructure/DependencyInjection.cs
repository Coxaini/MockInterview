using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Interviews.Application.Interviews.BackgroundJobs;
using MockInterview.Interviews.Infrastructure.BackgroundJobSetups;
using MockInterview.Interviews.Infrastructure.BackgroundServices;

namespace MockInterview.Interviews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<IndexCreationService>();
        services.ConfigureQuartzJobs();
        return services;
    }

    private static IServiceCollection ConfigureQuartzJobs(this IServiceCollection services)
    {
        services.ConfigureOptions<InterviewTimeOutJobSetup>();
        return services;
    }
}