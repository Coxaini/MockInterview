namespace MockInterview.Matchmaking.Domain.Models.Users;

[Flags]
public enum UserRole
{
    Interviewer = 1,
    Interviewee = 2,
}