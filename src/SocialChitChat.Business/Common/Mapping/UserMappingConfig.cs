using Mapster;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.Business.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, AppUserDto>()
            .Map(dest => dest.ProfilePictureUrl, src => src.GetMainProfilePictureUrl());

        config.NewConfig<AppUser, AppUserDetailDto>()
            .Map(dest => dest.ProfilePictureUrl, src => src.GetMainProfilePictureUrl());

        config.NewConfig<AppUser, AppUserWithRolesDto>()
            .Map(dest => dest.Roles, src => src.AppUserRoles.Select(ur => ur.AppRole.Name).ToList());

        config.NewConfig<UpdateAppUserDto, AppUser>();
    }
}
