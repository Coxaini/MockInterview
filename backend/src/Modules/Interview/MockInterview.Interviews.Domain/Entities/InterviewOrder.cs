using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class InterviewOrder : BaseEntity
{
    private InterviewOrder()
    {
    }

    public Guid CandidateId { get; private set; }
    public User Candidate { get; private set; } = null!;
    public DateTime StartDateTime { get; private set; }
    public string ProgrammingLanguage { get; private set; } = string.Empty;
    public string[] Technologies { get; private set; } = Array.Empty<string>();

    /*public Guid? InterviewId { get; private set; }*/
    public InterviewQuestionsList QuestionsList { get; private set; } = null!;

    public static InterviewOrder Create(Guid id, Guid candidateId, DateTime startDateTime, string programmingLanguage,
        IEnumerable<string> technologies)
    {
        return new InterviewOrder
        {
            Id = id,
            CandidateId = candidateId,
            StartDateTime = startDateTime,
            ProgrammingLanguage = programmingLanguage,
            Technologies = technologies.ToArray(),
            QuestionsList = InterviewQuestionsList.Create(id, candidateId)
        };
    }
}