using MockInterview.Interviews.Application.Abstractions.Repositories;
using MockInterview.Interviews.Domain.Entities;
using MockInterview.Interviews.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MockInterview.Interviews.Infrastructure.Persistence.Repositories;

public class InterviewRepository : IInterviewRepository
{
    private readonly IMongoCollection<Interview> _interviews;

    public InterviewRepository(InterviewMongoDbContext context)
    {
        _interviews = context.Interviews;
    }

    public Task AddAsync(Interview interview)
    {
        return _interviews.InsertOneAsync(interview);
    }

    public async Task<Interview?> GetByIdAsync(ObjectId id)
    {
        return await _interviews.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> PushQuestionAsync(ObjectId interviewId, Guid memberId, InterviewQuestion question)
    {
        var interview = await _interviews.Find(i => i.Id == interviewId).FirstOrDefaultAsync();

        if (interview == null) return false;

        UpdateDefinition<Interview> update;

        if (interview.FirstMemberId == memberId)
            update = Builders<Interview>.Update.Push(i => i.FirstMemberQuestions, question);
        else if (interview.SecondMemberId == memberId)
            update = Builders<Interview>.Update.Push(i => i.SecondMemberQuestions, question);
        else
            return false;

        await _interviews.UpdateOneAsync(i => i.Id == interviewId, update);
        return true;
    }

    public async Task<ShortInterview?> GetShortByIdAsync(ObjectId id)
    {
        var projection = Builders<Interview>.Projection.Expression(i => new ShortInterview
        {
            Id = i.Id,
            FirstMemberId = i.FirstMemberId,
            SecondMemberId = i.SecondMemberId,
            StartTime = i.StartTime,
            EndTime = i.EndTime,
            ProgrammingLanguage = i.ProgrammingLanguage,
            Tags = i.Tags
        });

        return await _interviews.Find(i => i.Id == id).Project(projection).FirstOrDefaultAsync();
    }
}