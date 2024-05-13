using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.DataAccess.Entities;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterLocalUserDto, LocalUser>();

        config.NewConfig<(LocalUser localUser, string token), AuthenticationDto>()
            .Map(dest => dest.LocalUserDto, src => src.localUser)
            .Map(dest => dest.Token, src => src.token);
    }
}
