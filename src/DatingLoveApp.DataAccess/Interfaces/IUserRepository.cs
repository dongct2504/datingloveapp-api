using DatingLoveApp.DataAccess.Identity;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IUserRepository : IRepository<AppUser>
{
    Task UpdateAsync(AppUser user);
}
