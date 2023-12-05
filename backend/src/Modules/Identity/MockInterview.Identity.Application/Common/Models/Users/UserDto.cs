using MockInterview.Identity.Domain.Entities.Users;

namespace MockInterview.Identity.Application.Common.Models.Users;

public record UserDto(
    Guid Id,
    string Name,
    string Username,
    string Email,
    string Bio,
    string? AvatarUrl,
    string? Location);