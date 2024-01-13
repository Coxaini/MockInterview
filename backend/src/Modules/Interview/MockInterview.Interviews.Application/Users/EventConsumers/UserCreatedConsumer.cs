using MassTransit;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Users.EventConsumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly InterviewsDbContext _dbContext;

    public UserCreatedConsumer(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;
        var user = User.Create(message.Id, message.Username, message.AvatarUrl, message.Name);

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}