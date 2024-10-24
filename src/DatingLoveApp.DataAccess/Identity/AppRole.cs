using Microsoft.AspNetCore.Identity;

namespace DatingLoveApp.DataAccess.Identity;

public class AppRole : IdentityRole<string>
{
    public AppRole() : base()
    {
    }

    public AppRole(string roleName) : base(roleName)
    {
    }

    public ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();
}
