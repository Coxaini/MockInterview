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
}