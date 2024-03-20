using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Models;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public record ChangeConferenceQuestionCommand(Guid UserId, Guid ConferenceId, Guid QuestionId)
    : IRequest<Result<ConferenceSessionDto>>;