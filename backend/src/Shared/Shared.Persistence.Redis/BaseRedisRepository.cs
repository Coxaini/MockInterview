using Redis.OM.Contracts;
using StackExchange.Redis;

namespace Shared.Persistence.Redis;

public abstract class BaseRedisRepository
{
    protected readonly IRedisConnectionProvider ConnectionProvider;

    protected BaseRedisRepository(IRedisConnectionProvider connectionProvider)
    {
        ConnectionProvider = connectionProvider;
    }
}