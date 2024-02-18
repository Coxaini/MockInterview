using MassTransit;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.Users.EventConsumers;

public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly InterviewsDbContext _dbContext;

    public UserUpdatedConsumer(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        var message = context.Message;
        var user = await _dbContext.Users.FindAsync(message.Id);

        if (user is null) return;

        user.UpdateInfo(message.Username, message.AvatarUrl, message.Name);

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}