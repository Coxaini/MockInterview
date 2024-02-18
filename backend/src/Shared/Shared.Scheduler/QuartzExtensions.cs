using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Shared.Scheduler;

public static class QuartzExtensions
{
    public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
}