using FluentResults;
using MediatR;

namespace MockInterview.Interviews.Application.Interviews.Commands;

public record EndInterviewCommand(Guid UserId, Guid InterviewId) : IRequest<Result>;