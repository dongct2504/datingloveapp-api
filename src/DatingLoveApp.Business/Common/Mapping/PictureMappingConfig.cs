using DatingLoveApp.Business.Dtos.PictureDtos;
using DatingLoveApp.DataAccess.Entities;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class PictureMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Picture, PictureDto>();
    }
}
