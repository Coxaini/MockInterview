using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Skills.Models;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public record GetUserSkillsQuery(Guid UserId) : IRequest<Result<UserSkillsDto>>;