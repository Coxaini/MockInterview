using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public record GetTechnologiesQuery : IRequest<Result<IEnumerable<Technology>>>;