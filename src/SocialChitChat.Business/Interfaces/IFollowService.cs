using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.FollowDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IFollowService
{
    Task<Result<PagedList<FollowerDto>>> GetFollowAsync(FollowParams likeParams);
    Task<bool> IsUserLikedAsync(Guid userSourceId, Guid userLikedId);
    Task<Result<bool>> UpdateLikeAsync(Guid sourceUserId, Guid likedUserId);
}
