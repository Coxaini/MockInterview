namespace Shared.Core.API.Interfaces;

public interface ICookieTokensService
{
    public void SetRefreshTokenCookie(string refreshToken, DateTime expirationDate);
    public void SetAccessTokenCookie(string accessToken, DateTime expirationDate);
    public void RemoveRefreshTokenCookie();
    public void RemoveAccessTokenCookie();
}