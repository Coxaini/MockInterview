using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MockInterview.Identity.Application.Abstractions.Interfaces;
using MockInterview.Identity.Infrastructure.Authentication.GitHub;
using MockInterview.Identity.Infrastructure.Security;

namespace MockInterview.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGitHubAuth(configuration);
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }


    private static IServiceCollection AddGitHubAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var githubOAuthSettings = new GitHubOAuthSettings();
        configuration.Bind("GitHubOAuthCredentials", githubOAuthSettings);
        configuration.Bind("GitHubOAuthEndpoints", githubOAuthSettings);
        services.AddSingleton(Options.Create(githubOAuthSettings));

        services.AddHttpClient<IGitHubAuthClient, GitHubAuthClient>();

        /*services.AddAuthentication("authentication")
            .AddCookie("authentication")
            .AddOAuth("github", o =>
            {
                o.ClientId = githubOAuthSettings.ClientId;
                o.ClientSecret = githubOAuthSettings.ClientSecret;
                o.AuthorizationEndpoint = githubOAuthSettings.AuthorizationEndpoint;
                o.TokenEndpoint = githubOAuthSettings.TokenEndpoint;
                o.UserInformationEndpoint = githubOAuthSettings.UserInformationEndpoint;
                o.CallbackPath = "/auth/github/callback";
                o.SignInScheme = "authentication";
                o.ClaimActions.MapJsonKey("sub", "id");
                o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                o.Events.OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    var response = await context.Backchannel.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

                    context.RunClaimActions(user);
                };
                o.Events.OnRemoteFailure = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/");
                    return Task.FromResult(0);
                };
            });*/

        return services;
    }
}