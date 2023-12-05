using MassTransit;
using Microsoft.Extensions.Logging;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models;

namespace MockInterview.Matchmaking.Application.Users.EventConsumers;

public sealed class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserUpdatedConsumer> _logger;

    public UserUpdatedConsumer(IUserRepository userRepository, ILogger<UserUpdatedConsumer> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        var @event = context.Message;
        var user = User.Create(@event.Id, @event.Username, @event.YearsOfExperience);
        bool isUpdated = await _userRepository.UpdateUserAsync(user);

        if (!isUpdated)
        {
            _logger.LogError("User with id {id} was not found in the database", user.Id);
        }
    }
}