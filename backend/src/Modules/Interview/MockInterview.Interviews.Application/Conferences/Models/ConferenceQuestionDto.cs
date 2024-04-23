namespace MockInterview.Interviews.Application.Conferences.Models;

public record ConferenceQuestionDto(
    Guid Id,
    string Text,
    string Tag,
    int OrderIndex);