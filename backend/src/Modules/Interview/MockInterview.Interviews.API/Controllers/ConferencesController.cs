using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MockInterview.Interviews.API.Hubs;
using MockInterview.Interviews.API.Requests;
using MockInterview.Interviews.API.Responses;
using MockInterview.Interviews.Application.Conferences.Commands;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Domain.Enumerations;
using Shared.Core.API.Controllers;

namespace MockInterview.Interviews.API.Controllers;

[ApiController]
[Route("conferences")]
public class ConferencesController : ApiController
{
    private readonly IHubContext<ConferenceHub, IConferenceClient> _hubContext;

    public ConferencesController(IMediator mediator, IHubContext<ConferenceHub, IConferenceClient> hubContext) :
        base(mediator)
    {
        _hubContext = hubContext;
    }

    /*[HttpPost("{interviewId:guid}/join")]
    public async Task<ActionResult<UserConferenceDto>> JoinConference(Guid interviewId)
    {
        var command = new JoinConferenceCommand(UserId, interviewId);
        var result = await Mediator.Send(command);

        return await MatchResultAsync(result, async conf =>
        {
            await _hubContext.Clients.User(conf.PeerId.ToString()).UserJoinedConference(conf.Id, UserId);
            return Ok(conf);
        });
    }*/

    [HttpPost("{interviewId:guid}/swap-roles")]
    public async Task<ActionResult<ConferenceRoleSwappedResponse>> SwapRole(Guid interviewId)
    {
        var command = new SwapConferenceRoleCommand(UserId, interviewId);
        var result = await Mediator.Send(command);

        return await MatchResultAsync(result, async conf =>
        {
            var peer = conf.Members.First(m => m.Id != UserId);
            var user = conf.Members.First(m => m.Id == UserId);
            var currentInterviewerQuestion =
                conf.Members.First(m => m.Role == ConferenceMemberRole.Interviewer).CurrentQuestion;
            await _hubContext.Clients.User(peer.Id.ToString())
                .RoleSwapped(new SwapRoleRequest(conf.Id, peer.Role, currentInterviewerQuestion));
            return Ok(new ConferenceRoleSwappedResponse(conf.Id, user.Role, user.CurrentQuestion));
        });
    }

    [HttpPost("{interviewId:guid}/change-question")]
    public async Task<ActionResult<UserConferenceDto>> ChangeQuestion(Guid interviewId, [FromBody] Guid questionId)
    {
        var command = new ChangeConferenceQuestionCommand(UserId, interviewId, questionId);

        var result = await Mediator.Send(command);

        return await MatchResultAsync(result, async conf =>
        {
            var peer = conf.Members.First(m => m.Id != UserId);
            var user = conf.Members.First(m => m.Id == UserId);
            if (user.CurrentQuestion is not null && user.Role == ConferenceMemberRole.Interviewer)
                await _hubContext.Clients.User(peer.Id.ToString()).QuestionChanged(conf.Id, user.CurrentQuestion);
            return Ok(conf);
        });
    }
}