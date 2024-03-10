using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MockInterview.Interviews.API.Hubs;
using MockInterview.Interviews.Application.Conferences.Commands;
using MockInterview.Interviews.Application.Conferences.Models;
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

    [HttpPost("{interviewId:guid}/join")]
    public async Task<ActionResult<UserConferenceDto>> JoinConference(Guid interviewId)
    {
        var command = new JoinConferenceCommand(UserId, interviewId);
        var result = await Mediator.Send(command);

        return await MatchResultAsync(result, async conf =>
        {
            await _hubContext.Clients.User(conf.PeerId.ToString()).UserJoinedConference(conf.Id, UserId);
            return Ok(conf);
        });
    }
}