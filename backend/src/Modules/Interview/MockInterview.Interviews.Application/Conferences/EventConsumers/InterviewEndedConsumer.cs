using MassTransit;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.Domain.Models;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace MockInterview.Interviews.Application.Conferences.EventConsumers;

public class InterviewEndedConsumer : IConsumer<InterviewEnded>
{
    private readonly IRedisCollection<ConferenceSession> _conferenceSessionCollection;

    public InterviewEndedConsumer(IRedisConnectionProvider connectionProvider)
    {
        _conferenceSessionCollection = connectionProvider.RedisCollection<ConferenceSession>();
    }

    public async Task Consume(ConsumeContext<InterviewEnded> context)
    {
        var interviewEnded = context.Message;

        var session = await _conferenceSessionCollection
            .FirstOrDefaultAsync(s => s.Id == interviewEnded.InterviewId);

        if (session is null)
            return;

        await _conferenceSessionCollection.DeleteAsync(session);
    }
}