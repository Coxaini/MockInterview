using Shared.Domain.Enums;

namespace MockInterview.Matchmaking.Domain.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public YearsCategory YearsOfExperience { get; private set; }

    private User()
    {
    }

    public static User Create(Guid id, string username, YearsCategory yearsOfExperience = YearsCategory.OneToThree)
    {
        return new User
        {
            Id = id,
            Username = username,
            YearsOfExperience = yearsOfExperience,
        };
    }
}