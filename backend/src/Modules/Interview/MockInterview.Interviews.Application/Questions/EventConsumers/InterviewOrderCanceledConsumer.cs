using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockInterview.Interviews.DataAccess;
using MockInterview.Matchmaking.Contracts.Events;

namespace MockInterview.Interviews.Application.Questions.EventConsumers;

public class InterviewOrderCanceledConsumer : IConsumer<InterviewOrderCanceled>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly ILogger<InterviewOrderCanceledConsumer> _logger;

    public InterviewOrderCanceledConsumer(InterviewsDbContext dbContext, ILogger<InterviewOrderCanceledConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<InterviewOrderCanceled> context)
    {
        var interviewQuestionsList = await _dbContext.InterviewQuestionsLists
            .FirstOrDefaultAsync(iql => iql.InterviewOrderId == context.Message.InterviewOrderId,
                context.CancellationToken);

        if (interviewQuestionsList is null) return;

        _dbContext.InterviewQuestionsLists.Remove(interviewQuestionsList);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("Interview questions list with id {InterviewQuestionsListId} was removed",
            interviewQuestionsList.Id);
    }
}