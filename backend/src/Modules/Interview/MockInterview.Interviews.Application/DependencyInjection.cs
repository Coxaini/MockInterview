using Microsoft.Extensions.DependencyInjection;
using MockInterview.Interviews.Application.Interviews.Services;

namespace MockInterview.Interviews.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IInterviewOrdersService, InterviewOrdersService>();

        return services;
    }
}