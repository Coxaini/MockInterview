using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Interviews.Services;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class CreateQuestionsListForOrderCommandHandler : IRequestHandler<
    CreateQuestionsListForOrderCommand,
    Result<InterviewQuestionsListDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateQuestionsListForOrderCommandHandler(InterviewsDbContext dbContext, IMapper mapper,
        IInterviewOrdersService ordersService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionsListDto>> Handle(CreateQuestionsListForOrderCommand request,
        CancellationToken cancellationToken)
    {
        var questionsList = await _dbContext.InterviewQuestionsLists
            .FirstOrDefaultAsync(l => l.InterviewOrderId == request.InterviewOrderId, cancellationToken);

        if (questionsList is not null)
        {
            if (questionsList.AuthorId != request.AuthorId)
                return Result.Fail(InterviewOrderErrors.NoAccessToOrder);

            return _mapper.Map<InterviewQuestionsListDto>(questionsList);
        }

        var order = await _dbContext.InterviewOrders
            .FirstOrDefaultAsync(o => o.Id == request.InterviewOrderId, cancellationToken);

        if (order is null)
            return Result.Fail(InterviewOrderErrors.OrderNotFound);

        if (order.CandidateId != request.AuthorId)
            return Result.Fail(InterviewOrderErrors.NoAccessToOrder);

        questionsList = InterviewQuestionsList.Create(request.InterviewOrderId, request.AuthorId);

        _dbContext.InterviewQuestionsLists.Add(questionsList);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InterviewQuestionsListDto>(questionsList);
    }
}