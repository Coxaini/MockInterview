using Mapster;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Interviews.Models;

public class InterviewMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Interview Interview, Guid UserId), InterviewDto>()
            .Map(x => x, s => s.Interview)
            .Map(x => x.StartDateTime, s => s.Interview.StartTime)
            .Map(x => x.EndDateTime, s => s.Interview.EndTime)
            .Map(x => x.Mate, s => s.Interview.GetMateOfMember(s.UserId).User);
    }
}