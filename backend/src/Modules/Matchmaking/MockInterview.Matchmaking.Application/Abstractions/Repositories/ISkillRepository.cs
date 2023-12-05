using System.Collections;
using MockInterview.Matchmaking.Domain.Models;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface ISkillRepository
{
    Task<bool> SkillsExistAsync(ICollection<string> skillNames);

    Task<ICollection<Skill>> GetSkillsAsync(IEnumerable<string> skillNames);

    Task<IEnumerable<Skill>> GetUsersSkillsAsync(Guid userId);

    Task<IEnumerable<Technology>> GetTechnologiesWithLanguagesAsync();
}