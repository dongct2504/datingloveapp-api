using SocialChitChat.Business.Common.Constants;
using SocialChitChat.DataAccess.Common.Enums;

namespace SocialChitChat.Business.Dtos.AppUsers;

public class UserParams : DefaultParams
{
    public string Name { get; set; } = string.Empty;

    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;

    public int MinAge { get; set; } = 16;
    public int MaxAge { get; set; } = 99;

    public string SortBy { get; set; } = UserSortConstants.LastActive;

    public override string? ToString()
    {
        return $"{Name}-{Gender}-{MinAge}-{MaxAge}-{SortBy}-{PageNumber}-{PageSize}";
    }
}
