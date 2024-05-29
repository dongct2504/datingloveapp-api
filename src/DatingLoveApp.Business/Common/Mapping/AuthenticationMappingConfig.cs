using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Extensions;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterLocalUserDto, LocalUser>();

        config.NewConfig<(LocalUser localUser, string token), AuthenticationDto>()
            .Map(dest => dest.LocalUserDto, src => src.localUser)
            .Map(dest => dest.LocalUserDto.ProfilePictureUrl, 
                src => src.localUser.Pictures.GetMainProfilePictureUrl())
            .Map(dest => dest.Token, src => src.token);
    }
}
