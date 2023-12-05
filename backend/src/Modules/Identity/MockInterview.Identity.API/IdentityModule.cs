using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Identity.Application;
using MockInterview.Identity.DataAccess;
using MockInterview.Identity.Infrastructure;
using Shared.Core.Mappings;

namespace MockInterview.Identity.API;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.ScanMappings(typeof(Application.DependencyInjection).Assembly);

        services
            .AddApplication()
            .AddDataBase(configuration)
            .AddInfrastructure(configuration);

        return services;
    }

    public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app)
    {
        return app;
    }
}