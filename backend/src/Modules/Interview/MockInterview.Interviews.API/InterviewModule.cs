using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Interviews.Application;
using MockInterview.Interviews.DataAccess;
using Shared.Core.Mappings;
using DependencyInjection = MockInterview.Interviews.Application.DependencyInjection;

namespace MockInterview.Interviews.API;

public static class InterviewModule
{
    public static IServiceCollection AddInterviewModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.ScanMappings(typeof(DependencyInjection).Assembly);

        services
            .AddApplication()
            .AddDataBase(configuration);

        return services;
    }

    public static IApplicationBuilder UseInterviewModule(this IApplicationBuilder app)
    {
        return app;
    }
}