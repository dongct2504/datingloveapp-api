using DatingLoveApp.Business.Dtos.AppUserLikes;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IAppUserLikeService
{
    Task<Result<IEnumerable<LikeDto>>> GetUserLikesAsync(string predicate, string userId);

    Task<Result> UpdateLikeAsync(string sourceUserId, string likedUserId);
}
