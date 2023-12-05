using MockInterview.Identity.Application.Abstractions.Models;

namespace MockInterview.Identity.Application.Abstractions.Interfaces;

public interface IGitHubAuthClient
{
    string GetAuthorizationEndpoint();

    Task<OAuthTokenResult> ExchangeCodeForAccessToken(string code);

    Task<GitHubUser> GetUser(string accessToken);
}