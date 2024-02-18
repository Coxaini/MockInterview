namespace MockInterview.Matchmaking.Contracts.Requests;

public record GetInterviewOrderRequest(Guid InterviewOrderId);

public record GetInterviewOrderResponse(
    Guid Id,
    Guid CandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies);

public record InterviewOrderNotFound(Guid Id);