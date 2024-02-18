using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record UpdateQuestionCommand(
    Guid UserId,
    Guid QuestionListId,
    Guid QuestionId,
    string Text,
    string Tag,
    DifficultyLevel DifficultyLevel,
    int OrderIndex) : IRequest<Result<InterviewQuestionDto>>;