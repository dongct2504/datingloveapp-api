using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    public PostRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(Post post)
    {
        _dbContext.Update(post);
    }
}
