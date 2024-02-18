using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.Questions.Queries;

public class GetQuestionsListQueryHandler : IRequestHandler<GetQuestionsListQuery, Result<InterviewQuestionsListDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetQuestionsListQueryHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionsListDto>> Handle(GetQuestionsListQuery request,
        CancellationToken cancellationToken)
    {
        var questionsList = await _dbContext.InterviewQuestionsLists
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.QuestionsListId, cancellationToken);

        if (questionsList is null)
            return Result.Fail(QuestionListErrors.QuestionListNotFound);

        return _mapper.Map<InterviewQuestionsListDto>(questionsList);
    }
}