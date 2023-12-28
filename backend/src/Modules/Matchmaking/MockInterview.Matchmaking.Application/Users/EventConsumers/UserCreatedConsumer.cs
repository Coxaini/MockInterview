using MassTransit;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Users;

namespace MockInterview.Matchmaking.Application.Users.EventConsumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IUserRepository _userRepository;

    public UserCreatedConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var user = User.Create(context.Message.Id, context.Message.Username);

        await _userRepository.AddUserAsync(user);
    }
}