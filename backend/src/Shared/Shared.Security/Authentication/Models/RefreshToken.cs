namespace Shared.Security.Authentication.Models;

public record RefreshToken(string Token = "", DateTime ExpiryTime = default);