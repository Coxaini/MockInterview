using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.MongoDB.Models;

namespace MockInterview.Interviews.Domain.Entities;

public class Interview : BsonEntity
{
    private List<string> _tags = new();

    private Interview()
    {
    }

    public Guid FirstMemberId { get; private set; }
    public Guid SecondMemberId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; }
    public string ProgrammingLanguage { get; private set; } = string.Empty;

    [BsonRepresentation(BsonType.String)] public IReadOnlyList<string> Tags => _tags.AsReadOnly();
    public List<InterviewQuestion> FirstMemberQuestions { get; }
    public List<InterviewQuestion> SecondMemberQuestions { get; }

    public static Interview Create(Guid firstMemberId, Guid secondMemberId, DateTime startTime,
        string programmingLanguage, IEnumerable<string> tags)
    {
        return new Interview
        {
            FirstMemberId = firstMemberId,
            SecondMemberId = secondMemberId,
            StartTime = startTime,
            ProgrammingLanguage = programmingLanguage,
            _tags = tags.ToList()
        };
    }
}