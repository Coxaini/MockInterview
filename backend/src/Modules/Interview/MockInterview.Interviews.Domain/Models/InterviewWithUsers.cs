using MockInterview.Interviews.Domain.Entities;
using MongoDB.Bson;

namespace MockInterview.Interviews.Domain.Models;

public class InterviewWithUsers
{
    public ObjectId Id { get; init; }
    public Guid FirstMemberId { get; init; }
    public Guid SecondMemberId { get; init; }
    public User FirstMember { get; init; } = null!;
    public User SecondMember { get; init; } = null!;
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public string ProgrammingLanguage { get; init; } = string.Empty;
    public IReadOnlyList<string> Tags { get; init; } = new List<string>();
}