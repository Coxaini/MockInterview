using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Identity.Application.Common.Errors;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.DataAccess;

namespace MockInterview.Identity.Application.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IdentityDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user is null)
        {
            return Result.Fail(UserErrors.UserNotFound);
        }

        return _mapper.Map<UserDto>(user);
    }
}