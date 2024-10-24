using Mapster;
using SocialChitChat.Business.Dtos.PictureDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.Business.Common.Mapping;

public class PictureMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Picture, PictureDto>();
    }
}
