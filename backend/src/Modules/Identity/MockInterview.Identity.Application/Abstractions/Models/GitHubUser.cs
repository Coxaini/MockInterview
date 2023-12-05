namespace MockInterview.Identity.Application.Abstractions.Models;

public record GitHubUser(
    long Id,
    string Login,
    string Name,
    string Email,
    string AvatarUrl,
    string Location,
    string Company,
    string Blog,
    string Bio
);