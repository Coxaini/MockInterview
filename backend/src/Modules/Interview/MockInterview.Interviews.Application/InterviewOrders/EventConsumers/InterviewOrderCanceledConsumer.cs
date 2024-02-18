using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.DataAccess;
using MockInterview.Matchmaking.Contracts.Events;

namespace MockInterview.Interviews.Application.InterviewOrders.EventConsumers;

public class InterviewOrderCanceledConsumer : IConsumer<InterviewOrderCanceled>
{
    private readonly InterviewsDbContext _dbContext;

    public InterviewOrderCanceledConsumer(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<InterviewOrderCanceled> context)
    {
        var interviewOrder = await _dbContext.InterviewOrders
            .FirstOrDefaultAsync(io => io.Id == context.Message.InterviewOrderId, context.CancellationToken);

        if (interviewOrder is null) return;

        _dbContext.InterviewOrders.Remove(interviewOrder);

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}