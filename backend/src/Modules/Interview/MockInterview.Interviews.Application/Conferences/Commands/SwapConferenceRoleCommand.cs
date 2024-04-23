using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public record SwapConferenceRoleCommand(Guid UserId, Guid ConferenceId) : IRequest<Result<ConferenceSessionDto>>;