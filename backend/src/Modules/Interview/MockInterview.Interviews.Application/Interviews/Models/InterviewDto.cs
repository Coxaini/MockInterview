using MongoDB.Bson;

namespace MockInterview.Interviews.Application.Interviews.Models;

public record InterviewDto(
    ObjectId Id,
    Guid FirstMemberId,
    Guid SecondMemberId,
    DateTime StartTime,
    DateTime? EndTime,
    string ProgrammingLanguage,
    IEnumerable<string> Tags
);