using MediatR;
using Microsoft.AspNetCore.SignalR;
using MockInterview.Interviews.Application.Conferences.Commands;

namespace MockInterview.Interviews.API.Hubs;

public class ConferenceHub : Hub<IConferenceClient>
{
    private readonly IMediator _mediator;

    public ConferenceHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.UserIdentifier is { } userIdentifier && Guid.TryParse(userIdentifier, out var userId))
        {
            var result = await _mediator.Send(new DisconnectFromConferenceCommand(userId));

            if (result is { IsSuccess: true, Value: { } activeConnections })
            {
                var tasks = activeConnections.Select(connection =>
                    Clients.User(connection.PeerId.ToString()).UserLeftConference(connection.ConferenceId, userId));

                await Task.WhenAll(tasks);
            }
        }
    }

    public Task SendOffer(Guid userId, string offer)
    {
        return Clients.User(userId.ToString()).ReceiveOffer(offer);
    }

    public Task SendAnswer(Guid userId, string answer)
    {
        return Clients.User(userId.ToString()).ReceiveAnswer(answer);
    }

    public Task SendIceCandidate(Guid userId, string iceCandidate)
    {
        return Clients.User(userId.ToString()).ReceiveIceCandidate(iceCandidate);
    }
}