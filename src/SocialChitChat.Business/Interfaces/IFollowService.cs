using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.FollowDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IFollowService
{
    Task<Result<List<FollowDto>>> GetAllFollowsAsync(Guid userId, string predicate);
    Task<Result<PagedList<FollowDto>>> GetFollowsAsync(FollowParams likeParams);
    Task<bool> IsUserFollowAsync(Guid userSourceId, Guid userLikedId);
    Task<Result<bool>> UpdateFollowAsync(Guid sourceUserId, Guid likedUserId);
}
