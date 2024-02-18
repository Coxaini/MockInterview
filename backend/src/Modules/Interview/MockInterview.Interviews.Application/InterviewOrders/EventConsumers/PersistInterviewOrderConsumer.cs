using MassTransit;
using MockInterview.Interviews.Contracts.Commands;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.InterviewOrders.EventConsumers;

public class PersistInterviewOrderConsumer : IConsumer<PersistInterviewOrder>
{
    private readonly InterviewsDbContext _dbContext;

    public PersistInterviewOrderConsumer(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<PersistInterviewOrder> context)
    {
        var command = context.Message;

        var interviewOrder = InterviewOrder.Create(command.InterviewOrderId, command.CandidateId,
            command.StartDateTime, command.ProgrammingLanguage, command.Technologies);

        _dbContext.InterviewOrders.Add(interviewOrder);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        await context.Publish(new InterviewOrderPersisted(interviewOrder.Id));
    }
}