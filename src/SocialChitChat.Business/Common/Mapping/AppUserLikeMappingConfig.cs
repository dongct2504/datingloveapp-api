using Mapster;
using SocialChitChat.Business.Dtos.AppUserLikes;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.Business.Common.Mapping;

public class AppUserLikeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUserLike, AppUserLikeDto>();
    }
}
