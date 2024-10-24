namespace SocialChitChat.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAppUserLikeRepository  AppUserLikes { get; }
    IPictureRepository Pictures { get; }

    IConversationRepository Conversations { get; }
    IMessageRepository Messages { get; }
    IParticipantRepository Participants { get; }

    Task SaveChangesAsync();
}
