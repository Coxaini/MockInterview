namespace MockInterview.Identity.Application.Authentication.Models;

public record RefreshTokenResult(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiryTime);