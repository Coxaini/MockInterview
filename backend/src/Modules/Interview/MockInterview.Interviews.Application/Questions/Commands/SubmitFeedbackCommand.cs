using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Questions.Models;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record SubmitFeedbackCommand(Guid UserId, Guid QuestionListId, Guid QuestionId, string Feedback)
    : IRequest<Result<InterviewQuestionDto>>;