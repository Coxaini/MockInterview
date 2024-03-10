using MockInterview.Interviews.Application.Conferences.Abstractions;
using MockInterview.Interviews.Domain.Models;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using Shared.Persistence.Redis;
using StackExchange.Redis;

namespace MockInterview.Interviews.Infrastructure.Repositories;

public class ConferenceConnectionRepository : BaseRedisRepository, IConferenceConnectionRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    /*private const string KeyPrefix = "conference-connection";

    public ConferenceConnectionRepository(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer)
    {
    }

    public async Task SetConnectionAsync(Guid interviewId, Guid userId)
    {
        await GetDatabase().StringSetAsync($"{KeyPrefix}:{userId}", interviewId.ToString());
    }

    public async Task RemoveConnectionAsync(Guid interviewId, Guid userId)
    {
        var db = GetDatabase();

        if (await db.StringGetAsync($"{KeyPrefix}:{userId}") == interviewId.ToString())
            await db.KeyDeleteAsync($"{KeyPrefix}:{userId}");
    }

    public async Task<Guid?> RemoveAnyUserConnectionAsync(Guid userId)
    {
        var db = GetDatabase();

        var conferenceId = await db.StringGetAsync($"{KeyPrefix}:{userId}");

        await db.KeyDeleteAsync($"{KeyPrefix}:{userId}");

        return conferenceId.HasValue ? Guid.Parse(conferenceId) : null;
    }

    public async Task<Guid?> GetConferenceByUserIdAsync(Guid userId)
    {
        var conferenceId = await GetDatabase().StringGetAsync($"{KeyPrefix}:{userId}");
        return conferenceId.HasValue ? Guid.Parse(conferenceId) : null;
    }*/

    private readonly IRedisCollection<ConferenceUser> _conferenceUsers;

    public ConferenceConnectionRepository(IRedisConnectionProvider connectionProvider,
        IConnectionMultiplexer connectionMultiplexer) : base(connectionProvider)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _conferenceUsers = connectionProvider.RedisCollection<ConferenceUser>();
    }

    public async Task AddConnectionAsync(ConferenceUser conferenceUser)
    {
        await _conferenceUsers.InsertAsync(conferenceUser);
    }

    public Task<ConferenceUser?> GetConnectionByUserIdAsync(Guid userId)
    {
        return _conferenceUsers.FirstOrDefaultAsync(c => c.Id == userId);
    }

    public async Task RemoveUserConnectionAsync(Guid userId)
    {
        await ConnectionProvider.Connection.UnlinkAsync($"ConferenceUser:{userId}");
    }
}