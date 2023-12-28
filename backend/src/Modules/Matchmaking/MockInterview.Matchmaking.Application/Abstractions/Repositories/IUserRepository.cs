using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;
using MockInterview.Matchmaking.Domain.Models.Users;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);

    Task<bool> UpdateUserAsync(User user);

    Task UpdateUserSkillsAsync(Guid userId, IEnumerable<Skill> skills);

    Task<bool> UserExistsAsync(Guid userId);
}