using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Application.Interviews.BackgroundJobs;
using MockInterview.Interviews.Application.Interviews.Services;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Models;
using Quartz;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public class
    DisconnectFromConferenceCommandHandler : IRequestHandler<DisconnectFromConferenceCommand,
    Result<IEnumerable<UserConnectionDto>>>
{
    private readonly IInterviewScheduler _scheduler;
    private readonly IRedisCollection<ConferenceSession> _conferenceSessionCollection;

    public DisconnectFromConferenceCommandHandler(IRedisConnectionProvider connectionProvider,
        IInterviewScheduler scheduler)
    {
        _scheduler = scheduler;
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

        var schedulerTasks = new List<Task>();

        foreach (var conference in conferences)
        {
            var member = conference.Members.First(m => m.Id == userId);

            member.IsConnected = false;

            var peer = conference.Members.First(m => m.Id != userId);

            userConnections.Add(new UserConnectionDto(peer.Id, conference.Id));

            if (conference.Members.All(m => !m.IsConnected))
                schedulerTasks.Add(_scheduler.ScheduleInterviewTimeOutAsync(conference.Id, userId,
                    DateBuilder.FutureDate(30, IntervalUnit.Minute), cancellationToken));
        }

        await _conferenceSessionCollection.SaveAsync();

        await Task.WhenAll(schedulerTasks);

        return userConnections;
    }
}