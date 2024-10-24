using Mapster;
using SocialChitChat.Business.Dtos.AuthenticationDtos;
using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.Business.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterAppUserDto, AppUser>();
    }
}
