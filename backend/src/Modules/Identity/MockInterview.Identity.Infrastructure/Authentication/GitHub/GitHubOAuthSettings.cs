namespace MockInterview.Identity.Infrastructure.Authentication.GitHub;

public class GitHubOAuthSettings
{
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
    public string AuthorizationEndpoint { get; init; } = "https://github.com/login/oauth/authorize";
    public string TokenEndpoint { get; init; } = "https://github.com/login/oauth/access_token";
    public string UserInformationEndpoint { get; init; } = "https://api.github.com/user";
    public string RedirectUri { get; init; } = null!;
    public string Scopes { get; init; } = null!;
}