using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Repositories;

namespace SocialChitChat.DataAccess.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly SocialChitChatDbContext _dbContext;

    public UnitOfWork(SocialChitChatDbContext dbContext)
    {
        _dbContext = dbContext;

        AppUserLikes = new AppUserLikeRepository(_dbContext);
        Pictures = new PictureRepository(_dbContext);

        GroupChats = new GroupChatRepository(_dbContext);
        Messages = new MessageRepository(_dbContext);
        Participants = new ParticipantRepository(_dbContext);
    }

    public IAppUserLikeRepository AppUserLikes { get; private set; }

    public IPictureRepository Pictures { get; private set; }

    public IGroupChatRepository GroupChats { get; private set; }
    public IMessageRepository Messages { get; private set; }
    public IParticipantRepository Participants { get; private set; }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
