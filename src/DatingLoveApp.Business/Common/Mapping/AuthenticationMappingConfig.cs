using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.DataAccess.Identity;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterAppUserDto, AppUser>();
    }
}
