using Microsoft.Extensions.Hosting;
using MockInterview.Interviews.Domain.Models;
using Redis.OM;
using Redis.OM.Contracts;

namespace MockInterview.Interviews.Infrastructure.BackgroundServices;

public class IndexCreationService : IHostedService
{
    private readonly IRedisConnectionProvider _connectionProvider;

    public IndexCreationService(IRedisConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    private readonly Type[] _types = { typeof(ConferenceSession), typeof(ConferenceUser) };

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var tasks = _types.Select(type => _connectionProvider.Connection.CreateIndexAsync(type));

        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}