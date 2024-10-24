using Microsoft.AspNetCore.Identity;

namespace SocialChitChat.DataAccess.Identity;

public class AppUserRole : IdentityUserRole<string>
{
    public AppUser AppUser { get; set; } = null!;

    public AppRole AppRole { get; set; } = null!;
}
