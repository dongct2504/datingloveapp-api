using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingLoveApp.DataAccess.Identity;

public class DatingLoveAppIdentityDbContext : IdentityDbContext<AppUser>
{
    public DatingLoveAppIdentityDbContext(DbContextOptions<DatingLoveAppIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
