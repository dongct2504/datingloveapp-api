using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.DataAccess.Identity;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, AppUserDto>();

        config.NewConfig<AppUser, AppUserDetailDto>();

        config.NewConfig<UpdateAppUserDto, AppUser>();
    }
}
