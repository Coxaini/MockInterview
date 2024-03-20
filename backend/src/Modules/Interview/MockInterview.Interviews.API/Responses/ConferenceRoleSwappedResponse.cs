using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.API.Responses;

public record ConferenceRoleSwappedResponse(
    Guid ConferenceId,
    ConferenceMemberRole NewRole,
    ConferenceQuestionDto? CurrentQuestion);