using Microsoft.Extensions.Options;
using MockInterview.Interviews.Domain.Entities;
using MockInterview.Interviews.Infrastructure.Persistence.Settings;
using MongoDB.Driver;

namespace MockInterview.Interviews.Infrastructure.Persistence;

public class InterviewMongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly InterviewMongoDbSettings _settings;

    public InterviewMongoDbContext(IMongoClient database, IOptions<InterviewMongoDbSettings> settings)
    {
        _settings = settings.Value;
        _database = database.GetDatabase(_settings.DataBaseName);
    }

    public IMongoCollection<Interview> Interviews =>
        _database.GetCollection<Interview>(_settings.InterviewsCollectionName);

    public IMongoCollection<Question> Questions =>
        _database.GetCollection<Question>(_settings.QuestionsCollectionName);
}