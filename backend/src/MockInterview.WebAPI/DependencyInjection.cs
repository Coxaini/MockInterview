using MockInterview.WebAPI.Services;
using Shared.Core.API.Interfaces;
using Shared.Core.Mappings;

namespace MockInterview.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICookieTokensService, CookieTokensService>();

        services.AddMappings();

        return services;
    }
}