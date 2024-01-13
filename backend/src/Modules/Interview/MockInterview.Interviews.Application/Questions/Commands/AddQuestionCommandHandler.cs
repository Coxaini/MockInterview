using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Result<InterviewQuestionModel>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public AddQuestionCommandHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionModel>> Handle(AddQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var interview = await _dbContext.Interviews
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview is null) return Result.Fail(InterviewErrors.InterviewNotFound);

        int maxOrderIndex = await _dbContext.InterviewQuestions
            .Where(q => q.InterviewId == request.InterviewId && q.AuthorId == request.UserId)
            .MaxAsync(q => q.OrderIndex, cancellationToken);

        var question = InterviewQuestion.Create(request.Text, request.UserId, request.DifficultyLevel,
            interview.ProgrammingLanguage, request.Tag, request.InterviewId,
            maxOrderIndex + 1);

        interview.AddQuestion(question);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(_mapper.Map<InterviewQuestionModel>(question));
    }
}