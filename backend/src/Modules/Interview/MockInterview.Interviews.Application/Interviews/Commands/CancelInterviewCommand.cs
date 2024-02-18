using FluentResults;
using MediatR;

namespace MockInterview.Interviews.Application.Interviews.Commands;

public record CancelInterviewCommand(Guid UserId, Guid InterviewId, string? CancelReason) : IRequest<Result>;