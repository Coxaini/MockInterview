using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Persistence.EfCore;

namespace MockInterview.Identity.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDbContext<IdentityDbContext>(configuration);

        return services;
    }
}