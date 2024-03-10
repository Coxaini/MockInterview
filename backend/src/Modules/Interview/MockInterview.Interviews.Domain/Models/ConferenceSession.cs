using Redis.OM.Modeling;

namespace MockInterview.Interviews.Domain.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "ConferenceSession" },
    IndexName = "ConferenceSessionIndex")]
public class ConferenceSession
{
    [RedisIdField] [Indexed] public required Guid Id { get; init; }

    public string? CurrentQuestion { get; set; }

    [Indexed(JsonPath = "$.Id")]
    public required ConferenceUser[] Members { get; init; } = Array.Empty<ConferenceUser>();
}