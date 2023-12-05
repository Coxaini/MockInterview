using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Common.Errors;
using MockInterview.Matchmaking.Application.Skills.Errors;

namespace MockInterview.Matchmaking.Application.Skills.Commands;

public class UpdateSkillsCommandHandler : IRequestHandler<UpdateSkillsCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ISkillRepository _skillRepository;

    public UpdateSkillsCommandHandler(IUserRepository userRepository, ISkillRepository skillRepository)
    {
        _userRepository = userRepository;
        _skillRepository = skillRepository;
    }

    public async Task<Result> Handle(UpdateSkillsCommand request, CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.GetSkillsAsync(request.Skills);

        if (skills.Count != request.Skills.Count())
        {
            return Result.Fail(SkillsErrors.SkillsNotValid);
        }

        if (!await _userRepository.UserExistsAsync(request.UserId))
        {
            return Result.Fail(UserErrors.UserNotFound);
        }

        await _userRepository.UpdateUserSkillsAsync(request.UserId, skills);

        return Result.Ok();
    }
}