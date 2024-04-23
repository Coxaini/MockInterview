using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.Interviews.Queries;

public class GetInterviewQueryHandler : IRequestHandler<GetInterviewQuery, Result<InterviewDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInterviewQueryHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewDto>> Handle(GetInterviewQuery request, CancellationToken cancellationToken)
    {
        var interview = await _dbContext.Interviews
            .Include(i => i.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview is null)
            return Result.Fail(InterviewErrors.InterviewNotFound);

        if (!interview.IsMember(request.UserId))
            return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

        return _mapper.Map<InterviewDto>((interview, request.UserId));
    }
}