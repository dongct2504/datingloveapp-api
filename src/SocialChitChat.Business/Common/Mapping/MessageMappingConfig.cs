using Mapster;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.Business.Common.Mapping;

public class MessageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Message, MessageDto>();
    }
}
