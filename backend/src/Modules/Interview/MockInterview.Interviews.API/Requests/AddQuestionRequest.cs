using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.API.Requests;

public record AddQuestionRequest(string Text, string Tag, DifficultyLevel DifficultyLevel);