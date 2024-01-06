using MockInterview.Interviews.Domain.Entities;
using MongoDB.Bson;

namespace MockInterview.Interviews.Application.Abstractions.Repositories;

public interface IInterviewRepository
{
    Task AddAsync(Interview interview);
    Task<Interview?> GetByIdAsync(ObjectId id);

    Task<bool> PushQuestionAsync(ObjectId interviewId, Guid memberId, InterviewQuestion question);
}