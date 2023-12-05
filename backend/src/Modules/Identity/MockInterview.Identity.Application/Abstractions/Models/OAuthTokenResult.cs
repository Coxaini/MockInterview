namespace MockInterview.Identity.Application.Abstractions.Models;

public record OAuthTokenResult(string AccessToken, string TokenType, string Scope);