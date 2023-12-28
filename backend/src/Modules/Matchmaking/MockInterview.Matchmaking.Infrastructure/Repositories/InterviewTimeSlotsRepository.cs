using Microsoft.Extensions.Options;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models.Interviews;
using Neo4j.Driver;
using Shared.Persistence.Neo4j.Common.Repositories;
using Shared.Persistence.Neo4j.Settings;

namespace MockInterview.Matchmaking.Infrastructure.Repositories;

public class InterviewTimeSlotsRepository : GraphRepository, IInterviewTimeSlotsRepository
{
    public InterviewTimeSlotsRepository(IDriver driver, IOptions<Neo4JSettings> settings) : base(driver, settings)
    {
    }

    public async Task<IList<InterviewTimeSlot>> GetInterviewTimeSlotsAsync(DateTime from, DateTime to,
        int startHour, int endHour, string programmingLanguage)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (:LANGUAGE { name: $Language })<-[:REQUIRES]-(ts:TIME_SLOT)
                WHERE datetime(ts.startsAt) >= $From AND datetime(ts.startsAt) <= $To AND
                      datetime(ts.startsAt).hour >= $StartHour AND datetime(ts.startsAt).hour <= $EndHour
                RETURN ts.startsAt AS startDateTime, ts.candidatesCount AS candidatesCount
                ORDER BY ts.startsAt
                """,
                new
                {
                    From = new ZonedDateTime(from.Date),
                    To = new ZonedDateTime(to.Date),
                    StartHour = startHour,
                    EndHour = endHour,
                    Language = programmingLanguage
                }
            );

            var result = await cursor.ToListAsync(record =>
            {
                var startDateTime = record["startDateTime"].As<DateTimeOffset>();
                var candidatesCount = record["candidatesCount"].As<int>();
                return new InterviewTimeSlot(startDateTime.DateTime, candidatesCount);
            });

            return result;
        });
    }
}