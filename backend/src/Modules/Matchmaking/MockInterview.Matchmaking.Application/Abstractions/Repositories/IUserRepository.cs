using MockInterview.Matchmaking.Domain.Models;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);

    Task<bool> UpdateUserAsync(User user);

    Task UpdateUserSkillsAsync(Guid userId, IEnumerable<Skill> skills);

    Task<bool> UserExistsAsync(Guid userId);
}