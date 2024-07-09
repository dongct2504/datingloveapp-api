using DatingLoveApp.Business.Dtos.MessageDtos;
using DatingLoveApp.DataAccess.Entities;
using Mapster;

namespace DatingLoveApp.Business.Common.Mapping;

public class MessageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Message, MessageDto>();
    }
}
