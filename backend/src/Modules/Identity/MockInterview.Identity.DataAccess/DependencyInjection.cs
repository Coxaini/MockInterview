using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistence.EfCore;

namespace MockInterview.Identity.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.Against.NullOrEmpty(connectionString);

        services.AddPostgresDbContext<IdentityDbContext>(configuration, connectionString,
            typeof(DependencyInjection).Assembly, IdentityDbContext.Schema);

        return services;
    }
}