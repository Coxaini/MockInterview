using MediatR;

namespace MockInterview.Matchmaking.API.Requests;

public record SignUpForInterviewRequest(
    string ProgrammingLanguage,
    IEnumerable<string> Technologies,
    DateTime StartTime) : IRequest;