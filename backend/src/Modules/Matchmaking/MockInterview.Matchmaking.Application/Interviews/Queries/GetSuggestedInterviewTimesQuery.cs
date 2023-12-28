using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Interviews.Models;

namespace MockInterview.Matchmaking.Application.Interviews.Queries;

public record GetSuggestedInterviewTimesQuery(TimeSpan TimeZoneOffset, string ProgrammingLanguage)
    : IRequest<Result<IEnumerable<InterviewTimeSlotDto>>>;