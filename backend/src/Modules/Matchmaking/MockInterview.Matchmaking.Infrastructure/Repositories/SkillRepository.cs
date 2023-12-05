using Microsoft.Extensions.Options;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models;
using Neo4j.Driver;
using Shared.Persistence.Neo4j.Common.Repositories;
using Shared.Persistence.Neo4j.Settings;

namespace MockInterview.Matchmaking.Infrastructure.Repositories;

public class SkillRepository : GraphRepository, ISkillRepository
{
    public SkillRepository(IDriver driver, IOptions<Neo4JSettings> settings) : base(driver, settings)
    {
    }

    public async Task<bool> SkillsExistAsync(ICollection<string> skillNames)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (s:LANGUAGE|TECHNOLOGY) WHERE s.name IN $Skills RETURN COUNT(s)",
                new
                {
                    Skills = skillNames
                }
            );

            var result = await cursor.SingleAsync();
            return result[0].As<int>() == skillNames.Count;
        });
    }

    public async Task<ICollection<Skill>> GetSkillsAsync(IEnumerable<string> skillNames)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (s:LANGUAGE|TECHNOLOGY) WHERE s.name IN $Skills RETURN s",
                new
                {
                    Skills = skillNames
                }
            );

            var result = await cursor.ToListAsync(r => new Skill
            {
                Name = r["s"].As<INode>().Properties["name"].As<string>(),
                Type = MapSkillType(r["s"].As<INode>().Labels[0])
            });

            return result;
        });
    }

    public async Task<IEnumerable<Skill>> GetUsersSkillsAsync(Guid userId)
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (u:USER {id: $Id})-[k:KNOWS]->(s) RETURN s",
                new
                {
                    Id = userId.ToString()
                }
            );

            var result = await cursor.ToListAsync(r => new Skill
            {
                Name = r["s"].As<INode>().Properties["name"].As<string>(),
                Type = MapSkillType(r["s"].As<INode>().Labels[0])
            });

            return result;
        });
    }

    public async Task<IEnumerable<Technology>> GetTechnologiesWithLanguagesAsync()
    {
        await using var session = OpenSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                """
                MATCH (t:TECHNOLOGY)
                OPTIONAL MATCH (t)<-[:USES]-(l:LANGUAGE)
                RETURN t.name as techName, collect(l.name) as langNames
                """
            );

            var result = await cursor.ToListAsync(r => new Technology
            {
                Name = r["techName"].As<string>(),
                ProgrammingLanguages = r["langNames"].As<List<string>>().ToArray()
            });

            return result;
        });
    }


    private static SkillType MapSkillType(string skillType)
    {
        return skillType switch
        {
            "LANGUAGE" => SkillType.Language,
            "TECHNOLOGY" => SkillType.Technology,
            _ => throw new ArgumentOutOfRangeException(nameof(skillType), skillType, "Unknown skill type")
        };
    }
}