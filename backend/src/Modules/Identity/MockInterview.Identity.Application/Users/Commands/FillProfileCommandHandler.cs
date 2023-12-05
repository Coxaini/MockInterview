using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Identity.Application.Common.Errors;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Identity.DataAccess;
using Shared.Messaging;

namespace MockInterview.Identity.Application.Users.Commands;

public class FillProfileCommandHandler : IRequestHandler<FillProfileCommand, Result<UserDto>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public FillProfileCommandHandler(IdentityDbContext dbContext, IMapper mapper, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<Result<UserDto>> Handle(FillProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Fail(UserErrors.UserNotFound);
        }

        user.UpdateUserInfo(request.Name, request.Location, request.YearsOfExperience, request.Bio);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new UserUpdatedEvent(user.Id, user.Name, user.YearsOfExperience));

        return _mapper.Map<UserDto>(user);
    }
}