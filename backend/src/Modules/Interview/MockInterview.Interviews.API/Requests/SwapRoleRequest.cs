using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.API.Requests;

public record SwapRoleRequest(Guid InterviewId, ConferenceMemberRole NewRole, ConferenceQuestionDto? CurrentQuestion);