using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Identity.API.Requests.Account;
using MockInterview.Identity.Application.Authentication.Account.Commands;
using MockInterview.Identity.Application.Authentication.Account.Queries;
using Shared.Core.API.Controllers;
using Shared.Core.API.Interfaces;

namespace MockInterview.Identity.API.Controllers;

[Route("auth")]
[AllowAnonymous]
public class UserAuthorizationController : ApiController
{
    private readonly ICookieTokensService _cookieTokensService;

    public UserAuthorizationController(ICookieTokensService cookieTokensService, IMediator mediator) : base(mediator)
    {
        _cookieTokensService = cookieTokensService;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        string? refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Refresh token is missing");
        }

        string? accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized("Access token is missing");
        }

        var result = await Mediator.Send(new RefreshAccessTokenQuery(accessToken, refreshToken));

        return MatchResult(result, (authenticationResult) =>
        {
            _cookieTokensService.SetAccessTokenCookie(authenticationResult.AccessToken,
                authenticationResult.RefreshTokenExpiryTime);

            return Ok();
        });
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        _cookieTokensService.RemoveAccessTokenCookie();
        _cookieTokensService.RemoveRefreshTokenCookie();

        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterByEmail(RegisterRequest request)
    {
        var result = await Mediator.Send(new RegisterByEmailCommand(request.Email, request.Username, request.Password));

        return MatchResult(result, (authenticationResult) =>
        {
            _cookieTokensService.SetAccessTokenCookie(authenticationResult.AccessToken,
                authenticationResult.RefreshTokenExpiryTime);
            _cookieTokensService.SetRefreshTokenCookie(authenticationResult.RefreshToken,
                authenticationResult.RefreshTokenExpiryTime);
            return Ok();
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginByEmail(LoginRequest request)
    {
        var result = await Mediator.Send(new LoginByEmailCommand(request.Email, request.Password));

        return MatchResult(result, (authenticationResult) =>
        {
            _cookieTokensService.SetAccessTokenCookie(authenticationResult.AccessToken,
                authenticationResult.RefreshTokenExpiryTime);
            _cookieTokensService.SetRefreshTokenCookie(authenticationResult.RefreshToken,
                authenticationResult.RefreshTokenExpiryTime);
            return Ok();
        });
    }
}