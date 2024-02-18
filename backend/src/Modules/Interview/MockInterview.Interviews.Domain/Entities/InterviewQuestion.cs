using MockInterview.Interviews.Domain.Enumerations;
using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class InterviewQuestion : BaseEntity
{
    private InterviewQuestion()
    {
    }

    public Guid InterviewQuestionsListId { get; private set; }
    public InterviewQuestionsList InterviewQuestionsList { get; private set; } = null!;
    public int OrderIndex { get; private set; }
    public int? Rating { get; private set; }
    public string? Feedback { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; private set; }
    public string Tag { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public static InterviewQuestion Create(InterviewQuestionsList list, string text,
        DifficultyLevel difficultyLevel, string tag, int orderIndex)
    {
        return new InterviewQuestion
        {
            Id = Guid.NewGuid(),
            InterviewQuestionsListId = list.Id,
            InterviewQuestionsList = list,
            Text = text,
            DifficultyLevel = difficultyLevel,
            Tag = tag,
            OrderIndex = orderIndex,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string text, string tag, DifficultyLevel difficultyLevel)
    {
        Text = text;
        Tag = tag;
        DifficultyLevel = difficultyLevel;
    }

    public void SetOrderIndex(int orderIndex)
    {
        OrderIndex = orderIndex;
    }

    public void SetFeedback(int rating, string feedback)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

        Rating = rating;
        Feedback = feedback;
    }
}