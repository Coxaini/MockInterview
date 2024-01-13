using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Questions.Models;

public record InterviewQuestionModel(
    Guid Id,
    string Text,
    string Tag,
    DifficultyLevel DifficultyLevel,
    DateTime CreatedAt,
    Guid InterviewId,
    Guid UserId);