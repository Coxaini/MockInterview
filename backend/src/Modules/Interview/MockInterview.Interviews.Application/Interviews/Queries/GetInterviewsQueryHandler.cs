using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Interviews.Queries;

public class GetInterviewsQueryHandler : IRequestHandler<GetInterviewsQuery, Result<UserInterviewsDataDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInterviewsQueryHandler(IMapper mapper, InterviewsDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<UserInterviewsDataDto>> Handle(GetInterviewsQuery request,
        CancellationToken cancellationToken)
    {
        var plannedInterviews = await _dbContext.Interviews
            .Include(i => i.Members)
            .ThenInclude(m => m.User)
            .Where(i => i.Members.Any(m => m.UserId == request.UserId) && i.EndTime == null)
            .OrderByDescending(i => i.StartTime)
            .ToListAsync(cancellationToken);


        var endedInterviews = await _dbContext.Interviews
            .Include(i => i.QuestionsLists.Where(l => l.AuthorId != request.UserId))
            .Include(i => i.Members)
            .ThenInclude(m => m.User)
            .Where(i => i.EndTime != null && i.Members.Any(m => m.UserId == request.UserId))
            .OrderByDescending(i => i.StartTime)
            .Take(5)
            .ToListAsync(cancellationToken);

        var result = new UserInterviewsDataDto(
            MapInterviews(plannedInterviews, request.UserId),
            MapInterviews(endedInterviews, request.UserId)
        );

        return Result.Ok(result);
    }

    private IEnumerable<UserInterviewDto> MapInterviews(IEnumerable<Interview> interviews, Guid userId)
    {
        return interviews.Select(i => new UserInterviewDto
        (
            i.Id,
            _mapper.Map<UserDto>(i.GetMateOfMember(userId).User),
            i.StartTime,
            i.EndTime,
            i.QuestionsLists.FirstOrDefault(l => l.AuthorId != userId)?.Feedback,
            i.ProgrammingLanguage,
            i.Tags
        ));
    }
}