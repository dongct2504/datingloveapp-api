using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    void Update(Post post);
}
