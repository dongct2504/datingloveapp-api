using DatingLoveApp.Business.Dtos.AppUsers;
using DatingLoveApp.DataAccess.Identity;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, AppUserDto>();

        config.NewConfig<AppUser, AppUserDetailDto>();

        config.NewConfig<AppUser, AppUserWithRolesDto>()
            .Map(dest => dest.Roles, src => src.AppUserRoles.Select(ur => ur.AppRole.Name).ToList());

        config.NewConfig<UpdateAppUserDto, AppUser>();
    }
}
