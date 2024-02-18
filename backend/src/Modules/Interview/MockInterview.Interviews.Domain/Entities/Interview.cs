using MockInterview.Interviews.Domain.Enumerations;
using Shared.Domain.Entities;

namespace MockInterview.Interviews.Domain.Entities;

public class Interview : BaseEntity
{
    private readonly List<InterviewMember> _members = new();
    private readonly List<InterviewQuestionsList> _questionsLists = new();

    private Interview()
    {
    }

    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; }
    public string ProgrammingLanguage { get; private set; } = string.Empty;
    public string[] Tags { get; private set; } = Array.Empty<string>();
    public IReadOnlyList<InterviewMember> Members => _members.AsReadOnly();
    public IReadOnlyList<InterviewQuestionsList> QuestionsLists => _questionsLists.AsReadOnly();

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

    /*public InterviewMember FirstMember => _members[0];
    public InterviewMember SecondMember => _members[1];*/

    public InterviewMember GetMateOfMember(Guid userId)
    {
        return _members.Single(m => m.UserId != userId);
    }

    public bool IsMember(Guid userId)
    {
        return _members.Any(m => m.UserId == userId);
    }

    public InterviewQuestionsList GetQuestionsListByUserId(Guid userId)
    {
        return _questionsLists.Single(ql => ql.AuthorId == userId);
    }

    public static Interview Create(Guid firstMemberId, Guid secondMemberId,
        Guid firstOrderId, Guid secondOrderId, DateTime startTime,
        string programmingLanguage, IEnumerable<string> tags)
    {
        var interviewId = Guid.NewGuid();
        return new Interview
        {
            Id = interviewId,
            _members =
            {
                InterviewMember.Create(firstMemberId, interviewId, firstOrderId),
                InterviewMember.Create(secondMemberId, interviewId, secondOrderId)
            },
            StartTime = startTime,
            ProgrammingLanguage = programmingLanguage,
            Tags = tags.ToArray()
        };
    }
}