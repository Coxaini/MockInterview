namespace MockInterview.Identity.Domain.Entities.Users;

[Flags]
public enum UserRole
{
    Interviewer = 1,
    Interviewee = 2,
}