using MockInterview.Interviews.Domain.Enumerations;
using Redis.OM.Modeling;

namespace MockInterview.Interviews.Domain.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "ConferenceUser" }, IndexName = "ConferenceUserIndex")]
public class ConferenceUser
{
    [RedisIdField] [Indexed] public required Guid Id { get; init; }
    public ConferenceMemberRole Role { get; set; }
    public bool IsConnected { get; set; }
}