using Microsoft.Extensions.Options;
using Neo4j.Driver;
using Shared.Persistence.Neo4j.Settings;

namespace Shared.Persistence.Neo4j.Common.Repositories;

public abstract class GraphRepository
{
    private readonly IDriver _driver;
    private readonly string _database;

    protected GraphRepository(IDriver driver, IOptions<Neo4JSettings> settings)
    {
        _driver = driver;
        _database = settings.Value.Database;
    }

    protected IAsyncSession OpenSession()
    {
        return _driver.AsyncSession(o => o.WithDatabase(_database));
    }
}