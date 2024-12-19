using Mapster;
using SocialChitChat.Business.Dtos.CommentDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;

namespace SocialChitChat.Business.Common.Mapping;

public class CommentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Comment, CommentDto>()
            .Map(dest => dest.SenderNickName, src => src.User.Nickname)
            .Map(dest => dest.SenderImageUrl, src => src.User.GetMainProfilePictureUrl());
    }
}
