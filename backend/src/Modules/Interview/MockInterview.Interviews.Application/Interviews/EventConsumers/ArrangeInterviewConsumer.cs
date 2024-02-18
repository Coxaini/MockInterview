using MassTransit;
using Microsoft.EntityFrameworkCore;
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
        var interview = Interview.Create(command.InitiatorCandidateId, command.MatchedCandidateId,
            command.InterviewOrderId, command.MatchedInterviewOrderId,
            command.StartDateTime, command.ProgrammingLanguage, command.MutualTechnologies);

        _dbContext.Interviews.Add(interview);

        var matchedQuestionsList = await _dbContext.InterviewQuestionsLists
            .FirstOrDefaultAsync(l => l.InterviewOrderId == command.MatchedInterviewOrderId,
                context.CancellationToken);

        if (matchedQuestionsList is not null) matchedQuestionsList.SetInterview(interview);

        var initiatorQuestionsList = await _dbContext.InterviewQuestionsLists
            .FirstOrDefaultAsync(l => l.InterviewOrderId == command.InterviewOrderId,
                context.CancellationToken);

        if (initiatorQuestionsList is not null) initiatorQuestionsList.SetInterview(interview);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        await context.Publish(new InterviewArranged(command.InterviewOrderId, interview.Id));
    }
}