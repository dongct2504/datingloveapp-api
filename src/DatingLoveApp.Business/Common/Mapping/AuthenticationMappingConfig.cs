using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Extensions;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterAppUserDto, LocalUser>();

        config.NewConfig<(LocalUser localUser, string token), AuthenticationDto>()
            .Map(dest => dest.AppUserDto, src => src.localUser)
            .Map(dest => dest.AppUserDto.ProfilePictureUrl, 
                src => src.localUser.Pictures.GetMainProfilePictureUrl())
            .Map(dest => dest.Token, src => src.token);
    }
}
