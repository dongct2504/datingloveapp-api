using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Enums;

namespace SocialChitChat.Business.Dtos.AppUsers;

public class UserParams
{
    public GenderEnums Gender { get; set; } = GenderEnums.Unknown;

    public int MinAge { get; set; } = 16;

    public int MaxAge { get; set; } = 99;

    public string SortBy { get; set; } = UserSortConstants.LastActive;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;

    public override string ToString()
    {
        return $"{Gender}-{MinAge}-{MaxAge}-{SortBy}-{PageNumber}-{PageSize}";
    }
}
