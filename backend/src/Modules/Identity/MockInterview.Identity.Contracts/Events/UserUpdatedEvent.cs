using Shared.Domain.Enums;

namespace MockInterview.Identity.Contracts.Events;

public record UserUpdatedEvent(Guid Id, string Username, YearsCategory YearsOfExperience);