using Mapster;
using MockInterview.Identity.Domain.Entities.Users;

namespace MockInterview.Identity.Application.Common.Models.Users;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>();
    }
}