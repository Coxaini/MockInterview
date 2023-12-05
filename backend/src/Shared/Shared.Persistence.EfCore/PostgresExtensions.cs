using System.Reflection;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Persistence.EfCore;

public static class PostgresExtensions
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, Assembly? migrationAssembly = null) where TDbContext : DbContext
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.NullOrEmpty(connectionString);

        services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString,
            sqlOptions =>
            {
                //sqlOptions.MigrationsAssembly(migrationAssembly?.GetName().Name ??
                //Assembly.GetExecutingAssembly().GetName().Name);

                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            }));

        services.BuildServiceProvider().GetService<TDbContext>()?.Database.EnsureCreated();

        return services;
    }
}