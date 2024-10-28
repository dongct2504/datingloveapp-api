using Mapster;
using SocialChitChat.Business.Dtos.ConversationDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.Business.Common.Mapping;

public class GroupChatMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GroupChat, GroupChatDto>();
    }
}
