using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Domain.Entities;

public class InterviewQuestion : Question
{
    private InterviewQuestion()
    {
    }

    public Guid InterviewId { get; private set; }
    public int OrderIndex { get; private set; }
    public int? Rating { get; private set; }
    public string? Feedback { get; private set; }

    public static InterviewQuestion Create(string text, Guid authorId, DifficultyLevel difficultyLevel,
        string programmingLanguage, string tag, Guid interviewId, int orderIndex)
    {
        return new InterviewQuestion
        {
            Text = text,
            AuthorId = authorId,
            DifficultyLevel = difficultyLevel,
            ProgrammingLanguage = programmingLanguage,
            Tag = tag,
            InterviewId = interviewId,
            OrderIndex = orderIndex
        };
    }

    public void SetFeedback(int rating, string feedback)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

        Rating = rating;
        Feedback = feedback;
    }
}