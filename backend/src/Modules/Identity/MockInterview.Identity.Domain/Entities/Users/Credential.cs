using MockInterview.Identity.Domain.Common.Enumerations;
using Shared.Domain.Entities;

namespace MockInterview.Identity.Domain.Entities.Users;

public class Credential : BaseEntity
{
    private Credential()
    {
    }

    public string ProviderUserId { get; private set; } = null!;
    public AuthProvider Provider { get; private set; }
    public string AccessToken { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public static Credential Create(string providerUserId, AuthProvider provider, string accessToken, User user)
    {
        return new Credential
        {
            ProviderUserId = providerUserId,
            Provider = provider,
            AccessToken = accessToken,
            UserId = user.Id,
            User = user
        };
    }

    public void UpdateAccessToken(string accessToken)
    {
        AccessToken = accessToken;
    }
}