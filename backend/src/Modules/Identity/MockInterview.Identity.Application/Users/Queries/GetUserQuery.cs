using FluentResults;
using MediatR;
using MockInterview.Identity.Application.Common.Models.Users;

namespace MockInterview.Identity.Application.Users.Queries;

public record GetUserQuery(Guid Id) : IRequest<Result<UserDto>>;