using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Models;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public class
    DisconnectFromConferenceCommandHandler : IRequestHandler<DisconnectFromConferenceCommand,
    Result<IEnumerable<UserConnectionDto>>>
{
    private readonly IRedisCollection<ConferenceSession> _conferenceSessionCollection;

    public DisconnectFromConferenceCommandHandler(IRedisConnectionProvider connectionProvider)
    {
        _conferenceSessionCollection = connectionProvider.RedisCollection<ConferenceSession>();
    }

    public async Task<Result<IEnumerable<UserConnectionDto>>> Handle(DisconnectFromConferenceCommand request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var conferences =
            await _conferenceSessionCollection
                .Where(c => c.Members.Any(m => m.Id == userId)).ToListAsync();

        var userConnections = new List<UserConnectionDto>();

        foreach (var conference in conferences)
        {
            var member = conference.Members.First(m => m.Id == userId);

            member.IsConnected = false;

            var peer = conference.Members.First(m => m.Id != userId);

            userConnections.Add(new UserConnectionDto(peer.Id, conference.Id));
        }

        await _conferenceSessionCollection.SaveAsync();

        return userConnections;
    }
}