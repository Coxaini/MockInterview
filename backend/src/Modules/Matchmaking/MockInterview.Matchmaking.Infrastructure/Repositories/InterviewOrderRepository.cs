using MassTransit.Testing;
using Microsoft.Extensions.Options;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models.Interviews;
using Neo4j.Driver;
using Shared.Persistence.Neo4j.Common.Repositories;
using Shared.Persistence.Neo4j.Settings;

namespace MockInterview.Matchmaking.Infrastructure.Repositories;

public class InterviewOrderRepository : GraphRepository, IInterviewOrderRepository
{
    public InterviewOrderRepository(IDriver driver, IOptions<Neo4JSettings> settings) : base(driver, settings)
    {
    }

    public async Task AddInterviewOrderAsync(InterviewOrder interview)
    {
        await using var session = OpenSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            var dateNow = DateTime.UtcNow;
            var cursor = await tx.RunAsync(
                """
                MATCH (u:USER {id: $CandidateId})
                MATCH (l:LANGUAGE {name: $ProgrammingLanguage})
                CREATE (i:INTERVIEW_ORDER {id: $Id})-[:REQUESTED_BY]->(u), (i)-[:REQUIRES]->(l)
                WITH i,l
                MERGE (dt:TIME_SLOT {startsAt: $StartDateTime})-[:REQUIRES]->(l)
                ON CREATE
                    SET dt.candidatesCount = 1
                ON MATCH
                    SET dt.candidatesCount = dt.candidatesCount + 1
                WITH i, dt
                CREATE (i)-[:SCHEDULED_AT {orderedAt: $DateNow}]->(dt)
                WITH i
                MATCH (techs:TECHNOLOGY) WHERE techs.name IN $Technologies
                CREATE (i)-[:REQUIRES]->(techs)
                """,
                new
                {
                    Id = interview.Id.ToString(),
                    interview.StartDateTime,
                    CandidateId = interview.CandidateId.ToString(),
                    interview.ProgrammingLanguage,
                    interview.Technologies,
                    DateNow = dateNow
                }
            );

            var result = await cursor.ConsumeAsync();
        });
    }

    public async Task<InterviewOrder?> GetInterviewOrderByIdAsync(Guid id)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (i:INTERVIEW_ORDER {id: $Id})-[:SCHEDULED_AT]->(ts:TIME_SLOT)
                MATCH (i)-[:REQUIRES]->(l:LANGUAGE)
                MATCH (i)-[:REQUESTED_BY]->(u:USER)
                OPTIONAL MATCH (i)-[:REQUIRES]->(t:TECHNOLOGY)
                RETURN i.id AS id, ts.startsAt AS startDateTime, u.id AS candidateId,
                 l.name AS programmingLanguage, collect(t.name) AS technologies
                """,
                new
                {
                    Id = id.ToString()
                }
            );

            var result = await cursor.FirstOrDefault();

            return result is null ? null : MapInterviewOrderFromRecord(result);
        });
    }

    public async Task<IList<InterviewOrder>> GetInterviewOrdersByStartDateTimeAsync(DateTime startsAt)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (i:INTERVIEW_ORDER)-[sch:SCHEDULED_AT]->(ts:TIME_SLOT {startsAt: $StartsAt})
                MATCH (i)-[:REQUIRES]->(l:LANGUAGE)
                MATCH (i)-[:REQUESTED_BY]->(u:USER)
                OPTIONAL MATCH (i)-[:REQUIRES]->(t:TECHNOLOGY)
                RETURN i.id AS id, ts.startsAt AS startDateTime, u.id AS candidateId,
                 l.name AS programmingLanguage, collect(t.name) AS technologies
                ORDER BY sch.orderedAt
                """,
                new
                {
                    StartsAt = new ZonedDateTime(startsAt)
                }
            );

            var result = await cursor.ToListAsync(MapInterviewOrderFromRecord);

            return result;
        });
    }

    public async Task<MatchedInterviewOrder?> GetBestMatchByMutualTechnologiesAsync(InterviewOrder matchOrder)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                WITH $Technologies AS matchTechnologies
                MATCH (i:INTERVIEW_ORDER)-[:SCHEDULED_AT]->(ts:TIME_SLOT {startsAt: $StartDateTime})
                WHERE i.id <> $OrderId AND (i)-[:REQUIRES]->(:LANGUAGE {name: $ProgrammingLanguage})
                 AND NOT (i)-[:REQUESTED_BY]->(:USER {id: $CandidateId})
                WITH i, matchTechnologies, ts
                OPTIONAL MATCH (i)-[:REQUIRES]->(t:TECHNOLOGY)
                WITH i, matchTechnologies, ts, collect(t.name) AS technologies
                WITH i, apoc.coll.intersection(technologies, matchTechnologies) AS mutualTechnologies, ts
                MATCH (i)-[:REQUESTED_BY]->(candidate:USER)
                RETURN i.id AS id, ts.startsAt AS startDateTime, mutualTechnologies, candidate.id as candidateId
                ORDER BY size(mutualTechnologies) DESC
                LIMIT 1
                """,
                new
                {
                    matchOrder.ProgrammingLanguage,
                    matchOrder.Technologies,
                    StartDateTime = new ZonedDateTime(matchOrder.StartDateTime),
                    OrderId = matchOrder.Id.ToString(),
                    CandidateId = matchOrder.CandidateId.ToString()
                }
            );

            var result = await cursor.FirstOrDefault();

            if (result is null) return null;

            var id = result["id"].As<string>();
            var startDateTime = result["startDateTime"].As<DateTimeOffset>();
            var candidateId = result["candidateId"].As<string>();
            string programmingLanguage = matchOrder.ProgrammingLanguage;
            var mutualTechnologies = result["mutualTechnologies"].As<List<string>>();

            return new MatchedInterviewOrder(Guid.Parse(id), Guid.Parse(candidateId), startDateTime.DateTime,
                programmingLanguage, mutualTechnologies);
        });
    }

    public async Task<IList<InterviewOrder>> GetInterviewOrdersAtDateTimeAsync(DateTime dateTime)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (i:INTERVIEW_ORDER)-[:SCHEDULED_AT]->(ts:TIME_SLOT)
                MATCH (i)-[:REQUIRES]->(l:LANGUAGE)
                MATCH (i)-[:REQUESTED_BY]->(u:USER)
                OPTIONAL MATCH (i)-[:REQUIRES]->(t:TECHNOLOGY)
                WHERE datetime(ts.startsAt) = $DateTime
                RETURN i.id AS id, ts.startsAt AS startDateTime, u.id AS candidateId,
                 l.name AS programmingLanguage, collect(t.name) AS technologies
                ORDER BY ts.startsAt
                """,
                new
                {
                    DateTime = new ZonedDateTime(dateTime.Date)
                }
            );

            var result = await cursor.ToListAsync(MapInterviewOrderFromRecord);

            return result;
        });
    }

    public async Task DeleteMatchInterviewOrdersAsync(Guid firstOrderId, Guid secondOrderId)
    {
        await using var session = OpenSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (i:INTERVIEW_ORDER)-[SCHEDULED_AT]->(ts:TIME_SLOT)
                WHERE i.id IN [$FirstOrderId, $SecondOrderId]
                CALL apoc.atomic.subtract(ts, 'candidatesCount', 1)
                YIELD oldValue, newValue
                DETACH DELETE i
                """,
                new
                {
                    FirstOrderId = firstOrderId.ToString(),
                    SecondOrderId = secondOrderId.ToString()
                }
            );

            await cursor.ConsumeAsync();
        });
    }

    public async Task DeleteInterviewOrderByIdAsync(Guid id)
    {
        await using var session = OpenSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (i:INTERVIEW_ORDER {id: $Id})-[SCHEDULED_AT]->(ts:TIME_SLOT)
                CALL apoc.atomic.subtract(ts, 'candidatesCount', 1)
                YIELD oldValue, newValue
                DETACH DELETE i
                """,
                new
                {
                    Id = id.ToString()
                }
            );

            await cursor.ConsumeAsync();
        });
    }

    public async Task<IList<InterviewOrder>> GetInterviewOrdersAsync(DateTime from, DateTime to, Guid candidateId)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (i:INTERVIEW_ORDER)-[:SCHEDULED_AT]->(ts:TIME_SLOT)
                MATCH (i)-[:REQUIRES]->(l:LANGUAGE)
                OPTIONAL MATCH (i)-[:REQUIRES]->(t:TECHNOLOGY)
                MATCH (i)-[:REQUESTED_BY]->(u:USER {id: $CandidateId})
                WHERE datetime(ts.startsAt) >= $From AND datetime(ts.startsAt) <= $To
                RETURN i.id AS id, ts.startsAt AS startDateTime, i.candidateId AS candidateId,
                 l.name AS programmingLanguage, collect(t.name) AS technologies
                ORDER BY ts.startsAt
                """,
                new
                {
                    From = new ZonedDateTime(from.Date),
                    To = new ZonedDateTime(to.Date),
                    CandidateId = candidateId.ToString()
                }
            );

            var result = await cursor.ToListAsync(MapInterviewOrderFromRecord);

            return result;
        });
    }

    private InterviewOrder MapInterviewOrderFromRecord(IRecord record)
    {
        var id = record["id"].As<string>();
        var startDateTime = record["startDateTime"].As<DateTimeOffset>();
        var candidateId = record["candidateId"].As<string>();
        var programmingLanguage = record["programmingLanguage"].As<string>();
        var technologies = record["technologies"].As<List<string>>();
        return new InterviewOrder(Guid.Parse(id), Guid.Parse(candidateId), startDateTime.UtcDateTime,
            programmingLanguage, technologies);
    }
}