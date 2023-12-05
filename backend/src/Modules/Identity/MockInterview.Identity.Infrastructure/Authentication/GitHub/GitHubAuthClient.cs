using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MockInterview.Identity.Application.Abstractions.Interfaces;
using MockInterview.Identity.Application.Abstractions.Models;
using MockInterview.Identity.Infrastructure.Authentication.Common.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MockInterview.Identity.Infrastructure.Authentication.GitHub;

public class GitHubAuthClient : IGitHubAuthClient
{
    private readonly GitHubOAuthSettings _gitHubOAuthSettings;
    private readonly HttpClient _httpClient;

    public GitHubAuthClient(HttpClient httpClient, IOptions<GitHubOAuthSettings> gitHubOAuthSettings)
    {
        _httpClient = httpClient;
        _gitHubOAuthSettings = gitHubOAuthSettings.Value;
    }

    public string GetAuthorizationEndpoint()
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["client_id"] = _gitHubOAuthSettings.ClientId,
            ["redirect_uri"] = _gitHubOAuthSettings.RedirectUri,
            ["scope"] = _gitHubOAuthSettings.Scopes,
            ["state"] = Guid.NewGuid().ToString("N")
        };

        return QueryHelpers.AddQueryString(_gitHubOAuthSettings.AuthorizationEndpoint, queryParams);
    }

    public async Task<OAuthTokenResult> ExchangeCodeForAccessToken(string code)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["client_id"] = _gitHubOAuthSettings.ClientId,
            ["client_secret"] = _gitHubOAuthSettings.ClientSecret,
            ["code"] = code,
            ["redirect_uri"] = _gitHubOAuthSettings.RedirectUri
        };

        using var request = new HttpRequestMessage(HttpMethod.Post,
            QueryHelpers.AddQueryString(_gitHubOAuthSettings.TokenEndpoint, queryParams));

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string query = await response.Content.ReadAsStringAsync();
        var responseParams = OAuthHelpers.ParseOAuthQuery(query);

        return responseParams;
    }

    public async Task<GitHubUser> GetUser(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _gitHubOAuthSettings.UserInformationEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("User-Agent", "MockInterview");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<GitHubUser>(content, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
        })!;
    }
}