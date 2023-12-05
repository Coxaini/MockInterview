using FluentResults;
using MediatR;

namespace MockInterview.Matchmaking.Application.Skills.Commands;

public record UpdateSkillsCommand(Guid UserId, IEnumerable<string> Skills) : IRequest<Result>;