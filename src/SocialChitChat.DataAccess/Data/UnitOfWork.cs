using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Repositories;

namespace SocialChitChat.DataAccess.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly SocialChitChatDbContext _dbContext;

    public UnitOfWork(SocialChitChatDbContext dbContext)
    {
        _dbContext = dbContext;

        Follows = new FollowRepository(_dbContext);
        Pictures = new PictureRepository(_dbContext);
        GroupChats = new GroupChatRepository(_dbContext);
        Messages = new MessageRepository(_dbContext);
        Participants = new ParticipantRepository(_dbContext);
        Posts = new PostRepository(_dbContext);
    }

    public IFollowRepository Follows { get; private set; }
    public IPictureRepository Pictures { get; private set; }
    public IGroupChatRepository GroupChats { get; private set; }
    public IMessageRepository Messages { get; private set; }
    public IParticipantRepository Participants { get; private set; }
    public IPostRepository Posts { get; private set; }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
