using Redis.OM.Modeling;

namespace MockInterview.Interviews.Domain.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "CurrentQuestion" })]
public class ConferenceQuestion
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public string Tag { get; init; } = string.Empty;
    public int OrderIndex { get; init; }
}