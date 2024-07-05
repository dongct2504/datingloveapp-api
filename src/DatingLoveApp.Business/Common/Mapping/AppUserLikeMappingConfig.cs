using DatingLoveApp.Business.Dtos.AppUserLikes;
using DatingLoveApp.DataAccess.Entities;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class AppUserLikeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUserLike, AppUserLikeDto>();
    }
}
