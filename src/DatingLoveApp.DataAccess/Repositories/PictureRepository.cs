using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.DataAccess.Repositories;

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
