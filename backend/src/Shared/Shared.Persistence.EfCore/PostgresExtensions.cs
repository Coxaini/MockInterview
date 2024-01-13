using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Persistence.EfCore;

public static class PostgresExtensions
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, string connectionString, Assembly? migrationAssembly = null,
        string? schema = null)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString,
            sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable($"__{typeof(TDbContext).Name}MigrationsHistory", schema);
                sqlOptions.MigrationsAssembly(migrationAssembly?.GetName().Name ??
                                              Assembly.GetExecutingAssembly().GetName().Name);

                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            }));

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        dbContext.Database.Migrate();

        return services;
    }
}