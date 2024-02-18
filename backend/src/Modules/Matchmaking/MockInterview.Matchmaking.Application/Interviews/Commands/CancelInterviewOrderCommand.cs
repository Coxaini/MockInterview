using FluentResults;
using MediatR;

namespace MockInterview.Matchmaking.Application.Interviews.Commands;

public record CancelInterviewOrderCommand(Guid InterviewOrderId, Guid UserId) : IRequest<Result>;