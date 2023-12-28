namespace MockInterview.Matchmaking.Domain.Models.Skills;

public class Skill
{
    public required string Name { get; init; }
    public required SkillType Type { get; init; }
}