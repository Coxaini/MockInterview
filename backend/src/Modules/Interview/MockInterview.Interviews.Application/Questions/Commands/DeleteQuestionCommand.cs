using FluentResults;
using MediatR;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record DeleteQuestionCommand(Guid UserId, Guid QuestionListId, Guid QuestionId) : IRequest<Result>;