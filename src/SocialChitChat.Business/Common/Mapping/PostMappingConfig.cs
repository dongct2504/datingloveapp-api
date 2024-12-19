using Mapster;
using SocialChitChat.Business.Dtos.PostDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;

namespace SocialChitChat.Business.Common.Mapping;

public class PostMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Post, PostDto>()
            .Map(dest => dest.PictureUrls, src => src.GetPostPictureUrls())
            .Map(dest => dest.LikeCount, src => src.Likes.Count)
            .Map(dest => dest.SenderNickName, src => src.User.Nickname)
            .Map(dest => dest.SenderImageUrl, src => src.User.GetMainProfilePictureUrl());

        config.NewConfig<Post, PostDetailDto>()
            .Map(dest => dest.PictureUrls, src => src.GetPostPictureUrls())
            .Map(dest => dest.LikeCount, src => src.Likes.Count)
            .Map(dest => dest.SenderNickName, src => src.User.Nickname)
            .Map(dest => dest.SenderImageUrl, src => src.User.GetMainProfilePictureUrl())
            .Map(dest => dest.CommentDtos, src => src.Comments);
    }
}
