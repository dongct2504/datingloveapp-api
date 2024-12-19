using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.PostDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IPostService
{
    public Task<PagedList<PostDto>> GetUserPostsAsync(GetUserPostsParams postsParams);
    public Task<Result<PostDetailDto>> GetPostAsync(Guid id);

    public Task<Result<PostDto>> CreatePostAsync(CreatePostDto dto);
    public Task<Result> UpdatePostAsync(UpdatePostDto dto);
    public Task<Result> RemoveAsync(Guid id);
}
