using Shared.MongoDB.Models;

namespace MockInterview.Interviews.Domain.Entities;

public class InterviewQuestion : BsonEntity
{
    private InterviewQuestion()
    {
    }

    public Question Question { get; private set; } = null!;
    public int? Rating { get; private set; }
    public string? Feedback { get; private set; }

    public static InterviewQuestion Create(Question question)
    {
        return new InterviewQuestion
        {
            Question = question
        };
    }

    public void SetFeedback(int rating, string feedback)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

        Rating = rating;
        Feedback = feedback;
    }
}