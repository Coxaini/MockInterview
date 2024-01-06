using MockInterview.Interviews.Domain.Enumerations;
using Shared.MongoDB.Models;

namespace MockInterview.Interviews.Domain.Entities;

public class Question : BsonEntity
{
    private Question()
    {
    }

    public string Text { get; private set; } = string.Empty;
    public Guid AuthorId { get; private set; }
    public DifficultyLevel DifficultyLevel { get; private set; }
    public string ProgrammingLanguage { get; private set; } = string.Empty;
    public string Technology { get; private set; } = string.Empty;

    public static Question Create(string text, Guid authorId, DifficultyLevel difficultyLevel,
        string programmingLanguage, string technology)
    {
        return new Question
        {
            Text = text,
            AuthorId = authorId,
            DifficultyLevel = difficultyLevel,
            ProgrammingLanguage = programmingLanguage,
            Technology = technology
        };
    }
}