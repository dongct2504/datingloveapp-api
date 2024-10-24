using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IAppUserLikeRepository : IRepository<AppUserLike>
{
    Task<AppUserLike?> GetUserLike(string sorceUserId, string likedUserId);

    Task UpdateAsync(AppUserLike appUserLike);
}
