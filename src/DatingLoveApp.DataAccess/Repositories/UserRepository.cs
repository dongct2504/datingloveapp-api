using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.DataAccess.Repositories;

public class UserRepository : Repository<AppUser>, IUserRepository
{
    public UserRepository(DatingLoveAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task UpdateAsync(AppUser user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
