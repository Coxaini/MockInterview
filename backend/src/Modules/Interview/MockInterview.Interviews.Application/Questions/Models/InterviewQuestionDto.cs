using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Questions.Models;

public record InterviewQuestionDto(
    Guid Id,
    string Text,
    string? Feedback,
    int? Rating,
    string Tag,
    DifficultyLevel DifficultyLevel,
    int OrderIndex);