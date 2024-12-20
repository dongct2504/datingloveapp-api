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
            .Map(dest => dest.SenderUserName, src => src.AppUser.UserName)
            .Map(dest => dest.SenderNickName, src => src.AppUser.Nickname)
            .Map(dest => dest.SenderImageUrl, src => src.AppUser.GetMainProfilePictureUrl())
            .Map(dest => dest.RecipientUserName, src =>
                src.Conversation.IsGroupChat
                    ? null // No single recipient in a group chat
                    : src.Conversation.Participants
                        .Where(p => p.AppUserId != src.SenderId)
                        .Select(p => p.AppUser.UserName)
                        .FirstOrDefault()
            )
            .Map(dest => dest.RecipientNickName, src =>
                src.Conversation.IsGroupChat
                    ? null
                    : src.Conversation.Participants
                        .Where(p => p.AppUserId != src.SenderId)
                        .Select(p => p.AppUser.Nickname)
                        .FirstOrDefault()
            )
            .Map(dest => dest.RecipientImageUrl, src =>
                src.Conversation.IsGroupChat
                    ? null
                    : src.Conversation.Participants
                        .Where(p => p.AppUserId != src.SenderId)
                        .Select(p => p.AppUser.GetMainProfilePictureUrl())
                        .FirstOrDefault()
            );
    }
}
