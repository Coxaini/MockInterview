using Mapster;
using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Interviews.Models;

public class InterviewOrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InterviewOrder, InterviewOrderDto>()
            .Map(dest => dest.Tags, src => src.Technologies);
    }
}