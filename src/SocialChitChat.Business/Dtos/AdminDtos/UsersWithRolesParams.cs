using SocialChitChat.Business.Common.Constants;

namespace SocialChitChat.Business.Dtos.AdminDtos;

public class UsersWithRolesParams
{
    public string SortBy { get; set; } = UserSortConstants.LastActive;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;

    public override string ToString()
    {
        return $"{SortBy}-{PageNumber}-{PageSize}";
    }
}
