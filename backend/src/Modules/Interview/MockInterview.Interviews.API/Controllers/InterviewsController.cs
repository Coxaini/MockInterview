using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Interviews.Application.Interviews.Commands;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Interviews.Queries;
using Shared.Core.API.Controllers;

namespace MockInterview.Interviews.API.Controllers;

[Route("interviews")]
public class InterviewsController : ApiController
{
    public InterviewsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInterviewDto>>> GetInterviews()
    {
        var result = await Mediator.Send(new GetInterviewsQuery(UserId));

        return MatchResult(result, Ok);
    }

    [HttpGet("{interviewId:guid}")]
    public async Task<ActionResult<InterviewDetailsDto>> GetInterview(Guid interviewId)
    {
        var result = await Mediator.Send(new GetInterviewDetailsQuery(UserId, interviewId));

        return MatchResult(result, Ok);
    }

    [HttpDelete("{interviewId:guid}")]
    public async Task<ActionResult> CancelInterview(Guid interviewId, [FromQuery] string? cancelReason = null)
    {
        var result = await Mediator.Send(new CancelInterviewCommand(UserId, interviewId, cancelReason));

        return MatchResult(result, NoContent);
    }
}