using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Interviews.API.Requests;
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
    public async Task<ActionResult<IEnumerable<UserInterviewDto>>> GetInterviews()
    {
        var result = await Mediator.Send(new GetInterviewsQuery(UserId));

        return MatchResult(result, Ok);
    }

    [HttpPost("{interviewId:guid}/questions")]
    public async Task<ActionResult> AddQuestion(Guid interviewId, AddQuestionRequest request)
    {
        var result = await Mediator.Send(new AddQuestionCommand(UserId,
            interviewId, request.Text, request.Tag, request.DifficultyLevel));

        return MatchResult(result, Ok);
    }
}