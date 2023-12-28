using System.Collections;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface ISkillRepository
{
    Task<bool> SkillsExistAsync(ICollection<string> skillNames);
    Task<ICollection<Skill>> GetSkillsAsync(IEnumerable<string> skillNames);
    Task<IEnumerable<Skill>> GetUsersSkillsAsync(Guid userId);
    Task<IEnumerable<string>> GetUsersProgrammingLanguagesAsync(Guid userId);
    Task<IEnumerable<Technology>> GetUserTechnologiesWithLanguagesAsync(Guid userId);
    Task<IEnumerable<Technology>> GetTechnologiesWithLanguagesAsync();
}