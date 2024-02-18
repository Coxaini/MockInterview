using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Interviews.Models;

namespace MockInterview.Interviews.Application.InterviewOrders.Queries;

public record GetInterviewOrderQuery(Guid UserId, Guid InterviewOrderId)
    : IRequest<Result<UpcomingInterviewDetailsDto>>;