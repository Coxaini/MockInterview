using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Interviews.Models;

namespace MockInterview.Matchmaking.Application.Interviews.Queries;

public class
    GetSuggestedInterviewTimesQueryHandler : IRequestHandler<GetSuggestedInterviewTimesQuery,
    Result<IEnumerable<InterviewTimeSlotDto>>>
{
    private readonly IInterviewTimeSlotsRepository _interviewTimeSlotsRepository;

    public GetSuggestedInterviewTimesQueryHandler(IInterviewTimeSlotsRepository interviewTimeSlotsRepository)
    {
        _interviewTimeSlotsRepository = interviewTimeSlotsRepository;
    }

    public async Task<Result<IEnumerable<InterviewTimeSlotDto>>> Handle(GetSuggestedInterviewTimesQuery request,
        CancellationToken cancellationToken)
    {
        var interviewTimeSlots = new List<InterviewTimeSlotDto>();

        var utcNow = DateTime.UtcNow;

        var userLocalDateTime = utcNow.Add(request.TimeZoneOffset);

        var startDateTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 8, 0, 0, DateTimeKind.Utc);

        var userStartDateTime = startDateTime.Add(request.TimeZoneOffset);
        var userEndDateTime = userStartDateTime.AddHours(12);

        var occupiedTimeSlots = await _interviewTimeSlotsRepository
            .GetInterviewTimeSlotsAsync(userStartDateTime.Date,
                userStartDateTime.Date.AddDays(7),
                userStartDateTime.Hour,
                userEndDateTime.Hour,
                request.ProgrammingLanguage);

        Dictionary<DateTime, int> occupiedTimeSlotsDictionary = new(occupiedTimeSlots.Count);
        foreach (var occupiedTimeSlot in occupiedTimeSlots)
            occupiedTimeSlotsDictionary.Add(occupiedTimeSlot.StartDateTime, occupiedTimeSlot.CandidatesCount);

        for (var i = 0; i < 7; i++)
        {
            var timeSlotDateTime = userStartDateTime.AddDays(i);
            var endTimeSlotDateTime = userEndDateTime.AddDays(i);
            while (timeSlotDateTime <= endTimeSlotDateTime)
            {
                if (userLocalDateTime.Date == timeSlotDateTime.Date &&
                    userLocalDateTime.TimeOfDay >= timeSlotDateTime.TimeOfDay)
                {
                    timeSlotDateTime = timeSlotDateTime.AddHours(1);
                    continue;
                }

                var recommendationLevel =
                    GetRecommendationLevel(occupiedTimeSlotsDictionary.GetValueOrDefault(timeSlotDateTime));
                var timeSlot = new InterviewTimeSlotDto(recommendationLevel, timeSlotDateTime);
                interviewTimeSlots.Add(timeSlot);
                timeSlotDateTime = timeSlotDateTime.AddHours(1);
            }
        }

        return interviewTimeSlots;
    }

    private static RecommendationLevel GetRecommendationLevel(int candidatesCount)
    {
        return candidatesCount switch
        {
            < 1 => RecommendationLevel.Low,
            < 3 => RecommendationLevel.Medium,
            _ => RecommendationLevel.High
        };
    }
}