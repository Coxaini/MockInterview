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
            .Include(i => i.Members)
            .ThenInclude(m => m.User)
            .Where(i => i.Members.Any(m => m.UserId == request.UserId))
            .ToListAsync(cancellationToken);

        var results = interviews.Select(i => new UserInterviewDto
        (
            i.Id,
            _mapper.Map<UserDto>(i.GetMateOfMember(request.UserId).User),
            i.StartTime,
            i.EndTime,
            i.ProgrammingLanguage,
            i.Tags
        ));

        return Result.Ok(results);
    }
}