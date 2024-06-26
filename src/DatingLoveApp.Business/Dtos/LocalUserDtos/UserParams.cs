using DatingLoveApp.Business.Common.Constants;

namespace DatingLoveApp.Business.Dtos.LocalUserDtos;

public class UserParams
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;

    public string Gender { get; set; } = GenderConstants.Unknown;

    public int MinAge { get; set; } = 16;

    public int MaxAge { get; set; } = 99;

    public override string ToString()
    {
        return $"{PageNumber}-{PageSize}-{Gender}-{MinAge}-{MaxAge}";
    }
}
