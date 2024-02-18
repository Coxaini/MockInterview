using FluentResults;
using MediatR;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record MoveQuestionCommand(Guid UserId, Guid QuestionsListId, Guid QuestionId, int NewIndex) : IRequest<Result>;