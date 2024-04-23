namespace MockInterview.Interviews.Application.Interviews.Models;

public record UserInterviewsDataDto(
    IEnumerable<UserInterviewDto> PlannedInterviews,
    IEnumerable<UserInterviewDto> EndedInterviews);