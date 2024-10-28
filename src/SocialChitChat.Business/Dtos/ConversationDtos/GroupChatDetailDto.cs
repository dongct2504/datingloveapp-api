using SocialChitChat.Business.Dtos.MessageDtos;

namespace SocialChitChat.Business.Dtos.ConversationDtos;

public class GroupChatDetailDto
{
    public Guid Id { get; set; }
    public string GroupName { get; set; } = null!;
    public bool IsGroupChat { get; set; }
    public List<Guid> ParticipantIds { get; set; } = new List<Guid>();
    public PagedList<MessageDto> PagedListMessageDto { get; set; } = new PagedList<MessageDto>();
}
