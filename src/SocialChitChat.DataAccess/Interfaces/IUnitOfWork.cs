namespace SocialChitChat.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAppUserLikeRepository  AppUserLikes { get; }
    IPictureRepository Pictures { get; }

    IGroupChatRepository GroupChats { get; }
    IMessageRepository Messages { get; }
    IParticipantRepository Participants { get; }

    Task SaveChangesAsync();
}
