using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingLoveApp.DataAccess.Identity;

public class DatingLoveAppIdentityDbContext 
    : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole,
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DatingLoveAppIdentityDbContext(DbContextOptions<DatingLoveAppIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.AppUserRoles)
            .WithOne(u => u.AppUser)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.AppUserRoles)
            .WithOne(u => u.AppRole)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }
}
