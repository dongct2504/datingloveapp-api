using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.DataAccess.Repositories;

public class UserRepository : Repository<LocalUser>, IUserRepository
{
    public UserRepository(DatingLoveAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task Update(LocalUser localUser)
    {
        _dbContext.Update(localUser);
        await _dbContext.SaveChangesAsync();
    }
}
