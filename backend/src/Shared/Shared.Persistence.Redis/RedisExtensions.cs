using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;

namespace Shared.Persistence.Redis;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        string? redisConnectionString = configuration.GetConnectionString("RedisConnection");

        Guard.Against.NullOrEmpty(redisConnectionString);

        services.AddSingleton<IConnectionMultiplexer>(opt =>
            ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddSingleton<IRedisConnectionProvider>(new RedisConnectionProvider(redisConnectionString));

        return services;
    }
}