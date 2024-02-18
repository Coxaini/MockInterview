using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.API.Requests;

public record UpdateQuestionRequest(string Text, string Tag, DifficultyLevel DifficultyLevel, int OrderIndex);