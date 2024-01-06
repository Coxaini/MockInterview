using MongoDB.Bson;

namespace MockInterview.Interviews.Domain.Models;

public class ShortInterview
{
    public ObjectId Id { get; init; }
    public Guid FirstMemberId { get; init; }
    public Guid SecondMemberId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public string ProgrammingLanguage { get; init; } = string.Empty;
    public IReadOnlyList<string> Tags { get; init; } = new List<string>();
}