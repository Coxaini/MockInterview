using MockInterview.Interviews.Domain.Enumerations;
using Redis.OM.Modeling;

namespace MockInterview.Interviews.Domain.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "ConferenceSession" },
    IndexName = "ConferenceSessionIndex")]
public class ConferenceSession
{
    [RedisIdField] [Indexed] public required Guid Id { get; init; }

    [Indexed(JsonPath = "$.Id")]
    public required ConferenceUser[] Members { get; init; } = Array.Empty<ConferenceUser>();

    public void SwapRoles()
    {
        foreach (var member in Members) member.SwapRole();
    }
}