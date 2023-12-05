using Shared.Domain.Enums;

namespace MockInterview.Identity.API.Requests.Users;

public record FillProfileRequest(string Name,
    string? Location,
    string? Bio,
    YearsCategory YearsOfExperience);