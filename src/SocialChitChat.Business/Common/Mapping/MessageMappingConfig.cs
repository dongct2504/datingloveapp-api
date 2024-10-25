using Mapster;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;

namespace SocialChitChat.Business.Common.Mapping;

public class MessageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Message, MessageDto>()
            .Map(dest => dest.SenderNickName, src => src.AppUser.Nickname)
            .Map(dest => dest.SenderImageUrl, src => src.AppUser.GetMainProfilePictureUrl());
    }
}
