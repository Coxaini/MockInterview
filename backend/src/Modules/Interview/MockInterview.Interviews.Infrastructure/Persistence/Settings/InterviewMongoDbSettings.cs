namespace MockInterview.Interviews.Infrastructure.Persistence.Settings;

public class InterviewMongoDbSettings
{
    public string ConnectionString { get; init; } = null!;
    public string DataBaseName { get; init; } = null!;
    public string InterviewsCollectionName { get; init; } = "interviews";
    public string QuestionsCollectionName { get; init; } = "questions";
}