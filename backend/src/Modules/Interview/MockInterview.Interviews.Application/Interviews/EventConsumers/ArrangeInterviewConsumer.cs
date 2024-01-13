using MassTransit;
using MockInterview.Interviews.Contracts.Commands;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Interviews.EventConsumers;

public class ArrangeInterviewConsumer : IConsumer<ArrangeInterview>
{
    private readonly InterviewsDbContext _dbContext;

    public ArrangeInterviewConsumer(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ArrangeInterview> context)
    {
        var command = context.Message;
        var interview = Interview.Create(command.FirstCandidateId, command.SecondCandidateId,
            command.StartDateTime, command.ProgrammingLanguage, command.Technologies);

        _dbContext.Interviews.Add(interview);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        await context.Publish(new InterviewArranged(command.InterviewOrderId, interview.Id.ToString()));
    }
}