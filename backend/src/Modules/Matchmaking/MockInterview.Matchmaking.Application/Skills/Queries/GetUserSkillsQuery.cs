using FluentResults;
using MediatR;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public record GetUserSkillsQuery(Guid UserId) : IRequest<Result<IEnumerable<string>>>;