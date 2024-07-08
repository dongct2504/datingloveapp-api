using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AppUserLikes;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IAppUserLikeService
{
    Task<Result<List<LikeDto>>> GetAllUserLikesAsync(string userId, string predicate);

    Task<Result<PagedList<LikeDto>>> GetUserLikesAsync(AppUserLikeParams likeParams);

    Task<Result<bool>> UpdateLikeAsync(string sourceUserId, string likedUserId);
}
