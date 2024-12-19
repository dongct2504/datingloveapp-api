using Mapster;
using SocialChitChat.Business.Dtos.FollowDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.Business.Common.Mapping;

public class FollowMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Follow, FollowDto>();
    }
}
