using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.DataAccess.Entities;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LocalUser, LocalUserDto>();

        config.NewConfig<UpdateLocalUserDto, LocalUser>();
    }
}
