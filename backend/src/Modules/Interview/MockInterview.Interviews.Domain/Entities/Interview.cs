using MockInterview.Interviews.Domain.Enumerations;
using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class Interview : BaseEntity
{
    private readonly List<InterviewQuestion> _memberQuestions = new();

    private Interview()
    {
    }

    public Guid FirstMemberId { get; private set; }
    public Guid SecondMemberId { get; private set; }

    public User FirstMember { get; } = null!;
    public User SecondMember { get; } = null!;
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; }
    public string ProgrammingLanguage { get; private set; } = string.Empty;
    public string[] Tags { get; private set; } = Array.Empty<string>();
    public IReadOnlyList<InterviewQuestion> Questions => _memberQuestions.AsReadOnly();

    public IReadOnlyList<InterviewQuestion> FirstMemberQuestions =>
        _memberQuestions.Where(x => x.AuthorId == FirstMemberId).ToList().AsReadOnly();

    public IReadOnlyList<InterviewQuestion> SecondMemberQuestions =>
        _memberQuestions.Where(x => x.AuthorId == SecondMemberId).ToList().AsReadOnly();

    public InterviewStatus Status
    {
        get
        {
            var now = DateTime.UtcNow;
            if (now < StartTime) return InterviewStatus.NotStarted;
            if (now > EndTime) return InterviewStatus.Finished;
            return InterviewStatus.InProgress;
        }
    }

    public void AddQuestion(InterviewQuestion question)
    {
        _memberQuestions.Add(question);
    }

    public static Interview Create(Guid firstMemberId, Guid secondMemberId, DateTime startTime,
        string programmingLanguage, IEnumerable<string> tags)
    {
        return new Interview
        {
            FirstMemberId = firstMemberId,
            SecondMemberId = secondMemberId,
            StartTime = startTime,
            ProgrammingLanguage = programmingLanguage,
            Tags = tags.ToArray()
        };
    }
}