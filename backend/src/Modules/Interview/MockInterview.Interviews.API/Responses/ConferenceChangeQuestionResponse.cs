using MockInterview.Interviews.Application.Conferences.Models;

namespace MockInterview.Interviews.API.Responses;

public record ConferenceChangeQuestionResponse(
    Guid ConferenceId,
    ConferenceQuestionDto? CurrentQuestion
);