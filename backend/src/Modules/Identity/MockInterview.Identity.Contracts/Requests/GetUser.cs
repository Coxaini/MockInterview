namespace MockInterview.Identity.Contracts.Requests;

public record GetUserRequest(Guid Id);

public record GetUserResponse(
    Guid Id,
    string Name,
    string Username,
    string? AvatarUrl
);

public record UserNotFoundResponse(Guid Id);