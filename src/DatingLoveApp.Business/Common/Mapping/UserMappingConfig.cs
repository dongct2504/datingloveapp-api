using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Dtos.PictureDtos;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Extensions;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LocalUser, LocalUserDto>()
            .Map(dest => dest.Age, src => src.DateOfBirth.GetAge())
            .Map(dest => dest.ProfilePictureUrl, src => src.Pictures.GetMainProfilePictureUrl());

        config.NewConfig<LocalUser, LocalUserDetailDto>()
            .Map(dest => dest.Age, src => src.DateOfBirth.GetAge())
            .Map(dest => dest.ProfilePictureUrl, src => src.Pictures.GetMainProfilePictureUrl());

        config.NewConfig<Picture, PictureDto>();

        config.NewConfig<UpdateLocalUserDto, LocalUser>();
    }
}
