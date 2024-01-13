using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Models;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.Interviews.Queries;

public class GetInterviewsQueryHandler : IRequestHandler<GetInterviewsQuery, Result<IEnumerable<UserInterviewDto>>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInterviewsQueryHandler(IMapper mapper, InterviewsDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<UserInterviewDto>>> Handle(GetInterviewsQuery request,
        CancellationToken cancellationToken)
    {
        var interviews = await _dbContext.Interviews
            .Include(i => i.FirstMember)
            .Include(i => i.SecondMember)
            .Where(i => i.FirstMemberId == request.UserId || i.SecondMemberId == request.UserId)
            .ToListAsync(cancellationToken);

        var results = interviews.Select(i => new UserInterviewDto
        (
            i.Id,
            _mapper.Map<UserDto>(i.FirstMemberId == request.UserId ? i.SecondMember : i.FirstMember),
            i.StartTime,
            i.EndTime,
            i.ProgrammingLanguage,
            i.Tags
        ));

        return Result.Ok(results);
    }
}