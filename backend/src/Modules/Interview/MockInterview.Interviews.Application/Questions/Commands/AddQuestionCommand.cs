using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record AddQuestionCommand(
    Guid UserId,
    Guid InterviewId,
    string Text,
    string Tag,
    DifficultyLevel DifficultyLevel) : IRequest<Result<InterviewQuestionModel>>;