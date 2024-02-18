namespace MockInterview.Interviews.Domain.Entities;

public class InterviewMember
{
    private InterviewMember()
    {
    }

    public Guid UserId { get; private set; }
    public Guid? InterviewOrderId { get; private set; }
    public Guid InterviewId { get; private set; }
    public Interview Interview { get; private set; } = null!;
    public InterviewOrder? InterviewOrder { get; }
    public User User { get; private set; } = null!;


    public static InterviewMember Create(Guid userId, Guid interviewId, Guid? interviewOrderId = null)
    {
        return new InterviewMember
        {
            UserId = userId,
            InterviewId = interviewId,
            InterviewOrderId = interviewOrderId
        };
    }
}