namespace SocialChitChat.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFollowRepository  Follows { get; }
    IPictureRepository Pictures { get; }
    IGroupChatRepository GroupChats { get; }
    IMessageRepository Messages { get; }
    IParticipantRepository Participants { get; }
    IPostRepository Posts { get; }

    Task SaveChangesAsync();
}
