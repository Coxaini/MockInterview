using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MockInterview.Interviews.API.Requests;
using MockInterview.Interviews.API.Responses;
using MockInterview.Interviews.Application.Conferences.Commands;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Application.Interviews.Commands;
using MockInterview.Interviews.Domain.Enumerations;
using Shared.Core.API.Hubs;

namespace MockInterview.Interviews.API.Hubs;

[Authorize]
public class ConferenceHub : BaseHub<IConferenceClient>
{
    private readonly IMediator _mediator;

    public ConferenceHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.UserIdentifier is not { } userIdentifier || !Guid.TryParse(userIdentifier, out var userId))
            return;

        var result = await _mediator.Send(new DisconnectFromConferenceCommand(userId));

        if (!result.IsSuccess || result.Value is null)
            return;

        var activeConnections = result.Value.ToArray();
        var tasks = new List<Task>(activeConnections.Length);

        foreach (var connection in activeConnections)
        {
            var conferenceId = connection.ConferenceId;
            var groupName = $"conf:{conferenceId}";

            tasks.Add(HandleConnectionDisconnect(groupName, conferenceId, userId));
        }

        await Task.WhenAll(tasks);
    }

    private async Task HandleConnectionDisconnect(string groupName, Guid conferenceId, Guid userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).UserLeftConference(conferenceId, userId);
    }

    public async Task<UserConferenceDto> JoinConference(Guid interviewId)
    {
        var userId = this.TryGetUserId();

        var command = new JoinConferenceCommand(userId, interviewId);
        var result = await _mediator.Send(command);

        if (result is { IsSuccess: true, Value: { } userConferenceDto })
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conf:{userConferenceDto.Id}");

            await Clients.OthersInGroup($"conf:{userConferenceDto.Id}")
                .UserJoinedConference(userConferenceDto.Id, userId);
            return userConferenceDto;
        }

        var firstError = result.Errors.First();
        throw new HubException(firstError.Message);
    }

    public async Task<ConferenceRoleSwappedResponse> SwapRoles(Guid interviewId)
    {
        var command = new SwapConferenceRoleCommand(UserId, interviewId);
        var result = await _mediator.Send(command);

        return await MatchResultAsync(result, async conf =>
        {
            var peer = conf.Members.First(m => m.Id != UserId);
            var user = conf.Members.First(m => m.Id == UserId);
            var currentInterviewerQuestion =
                conf.Members.First(m => m.Role == ConferenceMemberRole.Interviewer).CurrentQuestion;
            await Clients.OthersInGroup($"conf:{conf.Id}")
                .RoleSwapped(new SwapRoleRequest(conf.Id, peer.Role, currentInterviewerQuestion));
            return new ConferenceRoleSwappedResponse(conf.Id, user.Role, currentInterviewerQuestion);
        });
    }

    public async Task<ConferenceChangeQuestionResponse> ChangeQuestion(Guid interviewId, Guid questionId)
    {
        var command = new ChangeConferenceQuestionCommand(UserId, interviewId, questionId);

        var result = await _mediator.Send(command);

        return await MatchResultAsync(result, async conf =>
        {
            var user = conf.Members.First(m => m.Id == UserId);
            if (user.CurrentQuestion is not null && user.Role == ConferenceMemberRole.Interviewer)
                await Clients.OthersInGroup($"conf:{conf.Id}").QuestionChanged(conf.Id, user.CurrentQuestion);
            return new ConferenceChangeQuestionResponse(conf.Id, user.CurrentQuestion);
        });
    }

    public async Task EndConference(Guid interviewId)
    {
        var command = new EndInterviewCommand(UserId, interviewId);
        var result = await _mediator.Send(command);

        await MatchResultAsync(result, () => Clients.Group($"conf:{interviewId}").ConferenceEnded(interviewId));
    }

    public Task SendOffer(Guid conferenceId, string offer)
    {
        return Clients.OthersInGroup($"conf:{conferenceId}").ReceiveOffer(offer);
    }

    public Task SendAnswer(Guid conferenceId, string answer)
    {
        return Clients.OthersInGroup($"conf:{conferenceId}").ReceiveAnswer(answer);
    }

    public Task SendIceCandidate(Guid conferenceId, string iceCandidate)
    {
        return Clients.OthersInGroup($"conf:{conferenceId}").ReceiveIceCandidate(iceCandidate);
    }
}