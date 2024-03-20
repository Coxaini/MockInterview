namespace MockInterview.Interviews.Application.Conferences.Models;

public record ConferenceSessionDto(
    Guid Id,
    IEnumerable<ConferenceUserDto> Members);