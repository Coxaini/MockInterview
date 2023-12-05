using Shared.Core.API.Interfaces;

namespace MockInterview.WebAPI.Services;

public class CookieTokensService : ICookieTokensService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieTokensService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetRefreshTokenCookie(string refreshToken, DateTime expirationDate)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = expirationDate,
            Secure = true,
            SameSite = SameSiteMode.None,
        });
    }

    public void SetAccessTokenCookie(string accessToken, DateTime expirationDate)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = expirationDate,
            SameSite = SameSiteMode.None,
        });
    }

    public void RemoveRefreshTokenCookie()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
    }

    public void RemoveAccessTokenCookie()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
    }
}