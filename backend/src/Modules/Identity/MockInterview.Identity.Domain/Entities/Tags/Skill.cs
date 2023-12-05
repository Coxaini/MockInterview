using Shared.Domain.Entities;

namespace MockInterview.Identity.Domain.Entities.Tags;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public static Skill Create(string name)
    {
        return new Skill
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}