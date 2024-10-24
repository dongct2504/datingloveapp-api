using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUserLikes;

namespace SocialChitChat.Business.Interfaces;

public interface IAppUserLikeService
{
    Task<Result<List<LikeDto>>> GetAllUserLikesAsync(Guid userId, string predicate);
    Task<Result<PagedList<LikeDto>>> GetUserLikesAsync(AppUserLikeParams likeParams);

    Task<bool> IsUserLikedAsync(Guid userSourceId, Guid userLikedId);

    Task<Result<bool>> UpdateLikeAsync(Guid sourceUserId, Guid likedUserId);
}
