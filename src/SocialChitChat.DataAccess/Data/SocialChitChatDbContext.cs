using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.DataAccess.Data
{
    public partial class SocialChitChatDbContext : IdentityDbContext<AppUser, AppRole, Guid,
        IdentityUserClaim<Guid>, AppUserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public SocialChitChatDbContext(DbContextOptions<SocialChitChatDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUserLike> AppUserLikes { get; set; } = null!;

        public virtual DbSet<Picture> Pictures { get; set; } = null!;

        public virtual DbSet<GroupChat> GroupChats { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUserLike>()
                .HasKey(ul => new { ul.AppUserSourceId, ul.AppUserLikedId });
            modelBuilder.Entity<Participant>()
                .HasKey(ul => new { ul.ConversationId, ul.AppUserId });

            // relationship between user and userlike
            modelBuilder.Entity<AppUserLike>()
                .HasOne(ul => ul.AppUserSource)
                .WithMany(u => u.AppUserLikes)
                .HasForeignKey(ul => ul.AppUserSourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUserLike>()
                .HasOne(ul => ul.AppUserLiked)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(ul => ul.AppUserLikedId)
                .OnDelete(DeleteBehavior.NoAction); // manually clean up

            // config roles
            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.AppUserRoles)
                .WithOne(u => u.AppUser)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.AppUserRoles)
                .WithOne(u => u.AppRole)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}
