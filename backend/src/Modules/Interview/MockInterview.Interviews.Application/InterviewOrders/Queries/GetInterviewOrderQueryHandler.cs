using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.InterviewOrders.Errors;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.InterviewOrders.Queries;

public class
    GetInterviewOrderQueryHandler : IRequestHandler<GetInterviewOrderQuery, Result<UpcomingInterviewDetailsDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInterviewOrderQueryHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<UpcomingInterviewDetailsDto>> Handle(GetInterviewOrderQuery request,
        CancellationToken cancellationToken)
    {
        var interviewOrder = await _dbContext.InterviewOrders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(io => io.QuestionsList)
            .ThenInclude(ql => ql.Questions)
            .FirstOrDefaultAsync(io => io.Id == request.InterviewOrderId, cancellationToken);

        if (interviewOrder is null)
            return Result.Fail(InterviewOrderErrors.InterviewOrderNotFound);

        if (interviewOrder.CandidateId != request.UserId)
            return Result.Fail(InterviewOrderErrors.InterviewOrderNotOwnedByUser);

        var interviewDetails = new UpcomingInterviewDetailsDto(
            interviewOrder.Id,
            null,
            _mapper.Map<InterviewQuestionsListDto>(interviewOrder.QuestionsList),
            null,
            interviewOrder.StartDateTime,
            null,
            interviewOrder.ProgrammingLanguage,
            interviewOrder.Technologies
        );

        return interviewDetails;
    }
}