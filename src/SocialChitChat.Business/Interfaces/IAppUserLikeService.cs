using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUserLikes;

namespace SocialChitChat.Business.Interfaces;

public interface IAppUserLikeService
{
    Task<Result<List<LikeDto>>> GetAllUserLikesAsync(string userId, string predicate);
    Task<Result<PagedList<LikeDto>>> GetUserLikesAsync(AppUserLikeParams likeParams);

    Task<bool> IsUserLikedAsync(string userSourceId, string userLikedId);

    Task<Result<bool>> UpdateLikeAsync(string sourceUserId, string likedUserId);
}
