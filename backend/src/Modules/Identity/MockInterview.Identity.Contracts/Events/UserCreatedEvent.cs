namespace MockInterview.Identity.Contracts.Events;

public record UserCreatedEvent(Guid Id, string Email, string Username, string Name, string? AvatarUrl);