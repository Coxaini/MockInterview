using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Models;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public record JoinConferenceCommand(Guid UserId, Guid InterviewId) : IRequest<Result<UserConferenceDto>>;