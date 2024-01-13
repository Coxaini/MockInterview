using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Interviews.Models;

namespace MockInterview.Matchmaking.Application.Interviews.Queries;

public record GetInterviewOrdersQuery(Guid UserId) : IRequest<Result<IEnumerable<InterviewOrderDto>>>;