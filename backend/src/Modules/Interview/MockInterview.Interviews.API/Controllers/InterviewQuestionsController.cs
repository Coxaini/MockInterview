using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Interviews.API.Requests;
using MockInterview.Interviews.Application.Questions.Commands;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.Application.Questions.Queries;
using Shared.Core.API.Controllers;

namespace MockInterview.Interviews.API.Controllers;

[ApiController]
[Route("questions-lists")]
public class InterviewQuestionsController : ApiController
{
    public InterviewQuestionsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("from-order")]
    public async Task<ActionResult<InterviewQuestionsListDto>> CreateInterviewQuestionsListForOrder(
        [FromQuery] Guid interviewOrderId)
    {
        var result = await Mediator.Send(
            new CreateQuestionsListForOrderCommand(UserId, interviewOrderId));

        return MatchResult(result, Ok);
    }

    [HttpPost("from-interview")]
    public async Task<ActionResult> CreateInterviewQuestionsListForInterview([FromQuery] Guid interviewId)
    {
        var result = await Mediator.Send(
            new CreateQuestionsListForInterviewCommand(UserId, interviewId));

        return MatchResult(result, Ok);
    }

    [HttpPost("{questionsListId:guid}/questions")]
    public async Task<ActionResult> AddQuestion(Guid questionsListId, AddQuestionRequest request)
    {
        var result = await Mediator.Send(new AddQuestionCommand(UserId,
            questionsListId, request.Text, request.Tag, request.DifficultyLevel));

        return MatchResult(result, Ok);
    }

    [HttpDelete("{questionsListId:guid}/questions/{questionId:guid}")]
    public async Task<ActionResult> DeleteQuestion(Guid questionsListId, Guid questionId)
    {
        var result = await Mediator.Send(new DeleteQuestionCommand(UserId, questionsListId, questionId));

        return MatchResult(result, NoContent);
    }

    [HttpPut("{questionsListId:guid}/questions/{questionId:guid}")]
    public async Task<ActionResult> UpdateQuestion(Guid questionsListId, Guid questionId, UpdateQuestionRequest request)
    {
        var result = await Mediator.Send(new UpdateQuestionCommand(UserId, questionsListId, questionId, request.Text,
            request.Tag, request.DifficultyLevel, request.OrderIndex));

        return MatchResult(result, Ok);
    }

    [HttpPost("{questionsListId:guid}/questions/{questionId:guid}/feedback")]
    public async Task<ActionResult> SubmitFeedback(Guid questionsListId, Guid questionId, SubmitFeedbackRequest request)
    {
        var result =
            await Mediator.Send(new SubmitFeedbackCommand(UserId, questionsListId, questionId, request.Feedback));

        return MatchResult(result, Ok);
    }

    [HttpPost("{questionsListId:guid}/questions/{questionId:guid}/move")]
    public async Task<ActionResult> MoveQuestion(Guid questionsListId, Guid questionId, MoveQuestionRequest request)
    {
        var result =
            await Mediator.Send(new MoveQuestionCommand(UserId, questionsListId, questionId, request.NewIndex));

        return MatchResult(result, Ok);
    }

    [HttpGet("{questionsListId:guid}")]
    public async Task<ActionResult<InterviewQuestionsListDto>> GetQuestionsList(Guid questionsListId)
    {
        var result = await Mediator.Send(new GetQuestionsListQuery(UserId, questionsListId));

        return MatchResult(result, Ok);
    }
}