using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class PictureRepository : Repository<Picture>, IPictureRepository
{
    public PictureRepository(DatingLoveAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task UpdateAsync(Picture picture)
    {
        _dbContext.Update(picture);
        await _dbContext.SaveChangesAsync();
    }
}
