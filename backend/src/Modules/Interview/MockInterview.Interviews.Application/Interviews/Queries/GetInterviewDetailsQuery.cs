using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Interviews.Models;

namespace MockInterview.Interviews.Application.Interviews.Queries;

public record GetInterviewDetailsQuery(Guid UserId, Guid InterviewId) : IRequest<Result<UpcomingInterviewDetailsDto>>;