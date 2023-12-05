using System.Reflection;
using Ardalis.GuardClauses;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Core.Mappings;

public static class MappingsExtensions
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

    public static void ScanMappings(this IServiceCollection services, params Assembly[] assemblies)
    {
        var config = services.BuildServiceProvider().GetService<TypeAdapterConfig>();

        Guard.Against.Null(config);

        config.Scan(assemblies);
    }
}