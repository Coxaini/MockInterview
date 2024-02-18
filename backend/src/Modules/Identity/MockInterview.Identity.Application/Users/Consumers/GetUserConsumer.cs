using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockInterview.Identity.Contracts.Requests;
using MockInterview.Identity.DataAccess;

namespace MockInterview.Identity.Application.Users.Consumers;

public class GetUserConsumer : IConsumer<GetUserRequest>
{
    private readonly IdentityDbContext _dbContext;

    public GetUserConsumer(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetUserRequest> context)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == context.Message.Id);

        if (user is null)
        {
            await context.RespondAsync(new UserNotFoundResponse(context.Message.Id));
            return;
        }

        var response = new GetUserResponse(user.Id, user.Name, user.Username, user.AvatarUrl);

        await context.RespondAsync(response);
    }
}