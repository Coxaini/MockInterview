namespace MockInterview.Matchmaking.Domain.Models.Interviews;

public class InterviewOrder
{
    public InterviewOrder(Guid id, Guid candidateId, DateTime startDateTime, string programmingLanguage,
        IEnumerable<string> technologies)
    {
        Id = id;
        CandidateId = candidateId;
        StartDateTime = startDateTime;
        ProgrammingLanguage = programmingLanguage;
        Technologies = technologies;
    }

    public Guid Id { get; init; }
    public Guid CandidateId { get; init; }
    public DateTime StartDateTime { get; init; }
    public string ProgrammingLanguage { get; init; }
    public IEnumerable<string> Technologies { get; init; }
}