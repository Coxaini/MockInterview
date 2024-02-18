namespace MockInterview.Interviews.Application.Questions.Models;

public record InterviewQuestionsListDto(
    Guid Id,
    Guid? InterviewOrderId,
    Guid? InterviewId,
    Guid AuthorId,
    string? Feedback,
    IEnumerable<InterviewQuestionDto> Questions);