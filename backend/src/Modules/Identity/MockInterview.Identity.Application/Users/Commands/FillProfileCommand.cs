using FluentResults;
using MediatR;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.Domain.Entities.Users;
using Shared.Domain.Enums;

namespace MockInterview.Identity.Application.Users.Commands;

public record FillProfileCommand(Guid UserId,
    string Name,
    string? Location,
    string? Bio,
    YearsCategory YearsOfExperience) : IRequest<Result<UserDto>>;