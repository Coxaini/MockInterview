using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Models;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public record DisconnectFromConferenceCommand(Guid UserId) : IRequest<Result<IEnumerable<UserConnectionDto>>>;