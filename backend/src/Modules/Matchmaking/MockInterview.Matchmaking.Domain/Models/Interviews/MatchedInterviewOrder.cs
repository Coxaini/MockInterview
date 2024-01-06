namespace MockInterview.Matchmaking.Domain.Models.Interviews;

public class MatchedInterviewOrder
{
    public MatchedInterviewOrder(Guid interviewOrderId, Guid candidateId, DateTime startDateTime,
        string programmingLanguage, IEnumerable<string> mutualTechnologies)
    {
        InterviewOrderId = interviewOrderId;
        CandidateId = candidateId;
        StartDateTime = startDateTime;
        ProgrammingLanguage = programmingLanguage;
        MutualTechnologies = mutualTechnologies;
    }

    public Guid InterviewOrderId { get; init; }
    public Guid CandidateId { get; init; }
    public DateTime StartDateTime { get; init; }
    public string ProgrammingLanguage { get; init; }
    public IEnumerable<string> MutualTechnologies { get; init; }
}