using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IUserRepository : IRepository<LocalUser>
{
    Task UpdateAsync(LocalUser localUser);
}
