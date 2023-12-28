namespace MockInterview.Matchmaking.Domain.Models.Interviews;

public class InterviewTimeSlot
{
    public InterviewTimeSlot(DateTime startDateTime, int candidatesCount)
    {
        StartDateTime = startDateTime;
        CandidatesCount = candidatesCount;
    }

    public int CandidatesCount { get; init; }
    public DateTime StartDateTime { get; init; }
}