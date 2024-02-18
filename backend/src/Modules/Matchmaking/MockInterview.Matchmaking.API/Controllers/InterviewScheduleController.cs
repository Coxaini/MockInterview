using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Matchmaking.API.Requests;
using MockInterview.Matchmaking.Application.Interviews.Commands;
using MockInterview.Matchmaking.Application.Interviews.Models;
using MockInterview.Matchmaking.Application.Interviews.Queries;
using Shared.Core.API.Controllers;

namespace MockInterview.Matchmaking.API.Controllers;

[Route("schedule")]
public class InterviewScheduleController : ApiController
{
    public InterviewScheduleController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("plan-interview")]
    public async Task<ActionResult<InterviewOrderDto>> SignUpForInterview(SignUpForInterviewRequest request)
    {
        var result = await Mediator.Send(new OrderInterviewSlotCommand(UserId, request.ProgrammingLanguage,
            request.Technologies, request.StartTime));

        return MatchResult(result, Ok);
    }

    [HttpGet("requested-interviews")]
    public async Task<ActionResult<IEnumerable<InterviewOrderDto>>> GetRequestedInterviews()
    {
        var result = await Mediator.Send(new GetInterviewOrdersQuery(UserId));

        return MatchResult(result, Ok);
    }

    [HttpDelete("requested-interviews/{interviewOrderId}")]
    public async Task<ActionResult> CancelRequestedInterview(Guid interviewOrderId)
    {
        var result = await Mediator.Send(new CancelInterviewOrderCommand(interviewOrderId, UserId));

        return MatchResult(result, NoContent);
    }

    [HttpGet("suggested-times")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<InterviewTimeSlotDto>>> GetSuggestedInterviewTimes(
        [FromQuery] GetSuggestedInterviewTimesRequest request)
    {
        var result =
            await Mediator.Send(new GetSuggestedInterviewTimesQuery(TimeSpan.FromMinutes(request.TimeZoneOffset),
                request.ProgrammingLanguage));

        return MatchResult(result, Ok);
    }
}