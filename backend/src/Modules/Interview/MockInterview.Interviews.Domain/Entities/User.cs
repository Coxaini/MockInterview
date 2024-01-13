using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class User : BaseEntity
{
    private User()
    {
    }

    public string Name { get; private set; }
    public string Username { get; private set; }
    public string? AvatarUrl { get; private set; }

    public static User Create(Guid id, string username, string? avatarUrl = null, string? name = null)
    {
        return new User
        {
            Id = id,
            Username = username,
            AvatarUrl = avatarUrl,
            Name = name ?? username
        };
    }

    public void UpdateInfo(string username, string? avatarUrl = null, string? name = null)
    {
        Username = username;
        AvatarUrl = avatarUrl;
        Name = name ?? username;
    }
}