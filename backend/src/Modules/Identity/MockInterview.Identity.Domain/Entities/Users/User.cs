using MockInterview.Identity.Domain.Common.Enumerations;
using MockInterview.Identity.Domain.Entities.Tags;
using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace MockInterview.Identity.Domain.Entities.Users;

public class User : BaseEntity
{
    private readonly List<Credential> _credentials = new();

    private readonly List<Skill> _skills = new();

    private User()
    {
    }

    public string Name { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? AvatarUrl { get; private set; }
    public string? Location { get; private set; }
    public string? Company { get; private set; }
    public string? Blog { get; private set; }
    public YearsCategory YearsOfExperience { get; private set; }
    public string? Bio { get; private set; }
    public string? GitHubUrl { get; private set; }
    public string RefreshToken { get; private set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; private set; }
    public string? PasswordHash { get; private set; }
    public IReadOnlyList<Credential> Credentials => _credentials.AsReadOnly();

    public IReadOnlyList<Skill> Skills => _skills.AsReadOnly();

    public static User CreateWithAuthProvider(string name, string username, string email, string? avatarUrl,
        string? location, string? bio, AuthProvider provider,
        string accessToken, string providerUserId)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Username = username,
            Email = email,
            AvatarUrl = avatarUrl,
            Location = location,
            Bio = bio
        };
        user.AddCredential(provider, accessToken, providerUserId);
        return user;
    }

    public static User CreateWithPassword(string email, string username, string passwordHash)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = username,
            PasswordHash = passwordHash
        };
        return user;
    }

    public void UpdateUserInfo(string name, string? location, YearsCategory yearsOfExperience, string? bio)
    {
        Name = name;
        Location = location;
        Bio = bio;
        YearsOfExperience = yearsOfExperience;
    }

    public void UpdateSkills(IEnumerable<Skill> skills)
    {
        _skills.Clear();
        _skills.AddRange(skills);
    }

    public void AddCredential(AuthProvider provider, string accessToken, string providerUserId)
    {
        if (_credentials.Any(c => c.Provider == provider))
            throw new InvalidOperationException("User already has this credential");

        _credentials.Add(Credential.Create(providerUserId, provider, accessToken, this));
    }

    public void RemoveCredential(Credential credential)
    {
        if (_credentials.Count == 1)
            throw new InvalidOperationException("User must have at least one credential");

        _credentials.Remove(credential);
    }

    public void SetRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Invalid refresh token");

        if (refreshTokenExpiryTime < DateTime.UtcNow)
            throw new ArgumentException("Invalid refresh token expiry time");

        if (refreshToken == RefreshToken)
            throw new ArgumentException("Refresh token must be different");

        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}