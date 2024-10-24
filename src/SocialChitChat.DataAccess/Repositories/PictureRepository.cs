using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class PictureRepository : Repository<Picture>, IPictureRepository
{
    public PictureRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(Picture picture)
    {
        _dbContext.Update(picture);
    }
}
