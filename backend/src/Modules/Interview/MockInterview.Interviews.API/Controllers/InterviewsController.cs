using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Interviews.API.Requests;
using MockInterview.Interviews.Application.Interviews.Commands;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Interviews.Queries;
using MockInterview.Interviews.Application.Questions.Commands;
using Shared.Core.API.Controllers;

namespace MockInterview.Interviews.API.Controllers;

[Route("interviews")]
public class InterviewsController : ApiController
{
    public InterviewsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<UserInterviewsDataDto>> GetInterviews()
    {
        var result = await Mediator.Send(new GetInterviewsQuery(UserId));

        return MatchResult(result, Ok);
    }

    [HttpGet("{interviewId:guid}/details")]
    public async Task<ActionResult<InterviewDetailsDto>> GetInterviewDetails(Guid interviewId)
    {
        var result = await Mediator.Send(new GetInterviewDetailsQuery(UserId, interviewId));

        return MatchResult(result, Ok);
    }

    [HttpGet("{interviewId:guid}")]
    public async Task<ActionResult<UserInterviewDto>> GetInterview(Guid interviewId)
    {
        var result = await Mediator.Send(new GetInterviewQuery(UserId, interviewId));

        return MatchResult(result, Ok);
    }

    [HttpDelete("{interviewId:guid}")]
    public async Task<ActionResult> CancelInterview(Guid interviewId, [FromQuery] string? cancelReason = null)
    {
        var result = await Mediator.Send(new CancelInterviewCommand(UserId, interviewId, cancelReason));

        return MatchResult(result, NoContent);
    }

    [HttpPost("{interviewId:guid}/feedback")]
    public async Task<ActionResult<InterviewFeedbackDto>> SubmitFeedback(Guid interviewId,
        SubmitFeedbackRequest request)
    {
        var result = await Mediator.Send(new SubmitInterviewFeedbackCommand(UserId, interviewId, request.Feedback));

        return MatchResult(result, Ok);
    }
}