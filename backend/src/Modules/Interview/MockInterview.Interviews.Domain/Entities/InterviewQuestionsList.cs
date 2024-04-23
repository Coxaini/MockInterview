using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class InterviewQuestionsList : BaseEntity
{
    private readonly List<InterviewQuestion> _questions = new();

    private InterviewQuestionsList()
    {
    }

    public Guid? InterviewId { get; private set; }
    public Interview? Interview { get; private set; }
    public Guid AuthorId { get; private set; }
    public User Author { get; private set; } = null!;
    public Guid? InterviewOrderId { get; private set; }
    public InterviewOrder? InterviewOrder { get; }
    public IReadOnlyList<InterviewQuestion> Questions => _questions.AsReadOnly();
    public string? Feedback { get; private set; }
    public bool IsFeedbackSent { get; private set; } = false;

    public static InterviewQuestionsList Create(Guid interviewOrderId, Guid authorId)
    {
        return new InterviewQuestionsList
        {
            Id = Guid.NewGuid(),
            InterviewOrderId = interviewOrderId,
            AuthorId = authorId
        };
    }

    public void AddQuestion(InterviewQuestion question)
    {
        _questions.Add(question);
    }

    public void SetInterview(Interview interview)
    {
        InterviewId = interview.Id;
        Interview = interview;
    }

    public void SetFeedback(string feedback)
    {
        Feedback = feedback;
        IsFeedbackSent = true;
    }

    public static InterviewQuestionsList Create(Interview interview, Guid authorId, Guid? interviewOrderId)
    {
        return new InterviewQuestionsList
        {
            Id = Guid.NewGuid(),
            InterviewId = interview.Id,
            AuthorId = authorId,
            InterviewOrderId = interviewOrderId
        };
    }
}