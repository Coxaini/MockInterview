using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Identity.Application.Authentication.GitHub.Commands;
using MockInterview.Identity.Application.Authentication.GitHub.Queries;
using Shared.Core.API.Controllers;
using Shared.Core.API.Interfaces;

namespace MockInterview.Identity.API.Controllers;

[Route("auth/github")]
[AllowAnonymous]
public class GitHubAuthorizationController : ApiController
{
    private readonly ICookieTokensService _cookieTokensService;

    public GitHubAuthorizationController(IMediator mediator, ICookieTokensService cookieTokensService) : base(mediator)
    {
        _cookieTokensService = cookieTokensService;
    }

    [HttpGet("redirect")]
    public async Task<IActionResult> RedirectOnGitHubAuthServer()
    {
        return Redirect(await Mediator.Send(new GetGitHubAuthEndpointQuery()));
    }

    [HttpGet("callback")]
    public async Task<IActionResult> GitHubCallback([FromQuery] string code)
    {
        var result = await Mediator.Send(new AuthenticateGitHubUserCommand(code));

        return MatchResult(result, (authenticationResult) =>
        {
            _cookieTokensService.SetAccessTokenCookie(authenticationResult.AccessToken,
                authenticationResult.RefreshTokenExpiryTime);
            _cookieTokensService.SetRefreshTokenCookie(authenticationResult.RefreshToken,
                authenticationResult.RefreshTokenExpiryTime);
            if (authenticationResult.IsNewUser)
                return Redirect("http://localhost:4200/fill-profile");
            return Redirect("http://localhost:4200/");
        });
    }
}