namespace MockInterview.Matchmaking.API.Requests;

public record UpdateSkillsRequest(IEnumerable<string> ProgrammingLanguages, IEnumerable<string> Technologies);