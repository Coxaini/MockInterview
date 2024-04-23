using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Interviews.Models;

namespace MockInterview.Interviews.Application.Interviews.Queries;

public record GetInterviewsQuery(Guid UserId) : IRequest<Result<UserInterviewsDataDto>>;