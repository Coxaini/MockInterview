using MockInterview.Interviews.Domain.Enumerations;
using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class Question : BaseEntity
{
    protected Question()
    {
    }

    public string Text { get; protected set; } = string.Empty;
    public User Author { get; protected set; } = null!;
    public Guid AuthorId { get; protected set; }
    public DifficultyLevel DifficultyLevel { get; protected set; }
    public string Tag { get; protected set; } = string.Empty;
    public DateTime CreatedAt { get; protected set; }

    public static Question Create(string text, Guid authorId, DifficultyLevel difficultyLevel, string tag)
    {
        return new Question
        {
            Id = Guid.NewGuid(),
            Text = text,
            AuthorId = authorId,
            DifficultyLevel = difficultyLevel,
            Tag = tag,
            CreatedAt = DateTime.UtcNow
        };
    }
}