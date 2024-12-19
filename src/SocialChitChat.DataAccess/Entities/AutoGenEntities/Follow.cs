using SocialChitChat.DataAccess.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities
{
    public partial class Follow
    {
        [Key]
        public Guid FollowerId { get; set; } // user who follows
        [Key]
        public Guid FollowingId { get; set; } // user being followed
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public virtual AppUser Follower { get; set; } = null!;
        public virtual AppUser Following { get; set; } = null!;
    }
}
