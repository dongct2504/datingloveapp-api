using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

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
