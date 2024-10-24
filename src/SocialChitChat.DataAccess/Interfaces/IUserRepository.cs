using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IUserRepository : IRepository<AppUser>
{
    Task UpdateAsync(AppUser user);
}
