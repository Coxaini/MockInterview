using Microsoft.Extensions.Options;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;
using MockInterview.Matchmaking.Domain.Models.Users;
using Neo4j.Driver;
using Shared.Persistence.Neo4j.Common.Repositories;
using Shared.Persistence.Neo4j.Settings;

namespace MockInterview.Matchmaking.Infrastructure.Repositories;

public class UserRepository : GraphRepository, IUserRepository
{
    public UserRepository(IDriver driver, IOptions<Neo4JSettings> settings) : base(driver, settings)
    {
    }

    public async Task AddUserAsync(User user)
    {
        await using var session = OpenSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "CREATE (u:USER {id: $Id, name: $Name, experienceCategory: $ExperienceCategory})",
                new
                {
                    Id = user.Id.ToString(),
                    Name = user.Username,
                    ExperienceCategory = (int)user.YearsOfExperience,
                }
            );

            var result = await cursor.ConsumeAsync();
        });
    }

    public async Task UpdateUserSkillsAsync(Guid userId, IEnumerable<Skill> skills)
    {
        await using var session = OpenSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (u:USER {id: $Id})
                OPTIONAL MATCH (u)-[k:KNOWS]->(s)
                DELETE k
                WITH u
                MATCH (s:LANGUAGE|TECHNOLOGY) WHERE s.name IN $Skills
                MERGE (u)-[:KNOWS]->(s)
                """,
                new
                {
                    Id = userId.ToString(),
                    Skills = skills.Select(s => s.Name),
                }
            );

            await cursor.ConsumeAsync();
        });
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (u:USER {id: $Id}) RETURN COUNT(u)",
                new
                {
                    Id = userId.ToString(),
                }
            );

            var result = await cursor.SingleAsync();
            return result[0].As<int>() == 1;
        });
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        await using var session = OpenSession();

        return await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (u:USER {id: $Id}) SET u.name = $Name, u.experienceCategory = $ExperienceCategory",
                new
                {
                    Id = user.Id.ToString(),
                    Name = user.Username,
                    ExperienceCategory = (int)user.YearsOfExperience,
                }
            );

            var result = await cursor.ConsumeAsync();

            return result.Counters.ContainsUpdates;
        });
    }
}