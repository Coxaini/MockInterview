using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;

namespace MockInterview.Matchmaking.Application.Skills.Models;

public record UserSkillsDto(IEnumerable<string> ProgrammingLanguages, IEnumerable<Technology> Technologies);