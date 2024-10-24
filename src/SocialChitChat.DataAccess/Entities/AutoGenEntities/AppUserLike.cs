using SocialChitChat.DataAccess.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities
{
    public partial class AppUserLike
    {
        [Key]
        public Guid AppUserSourceId { get; set; }
        [Key]
        public Guid AppUserLikedId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public virtual AppUser AppUserSource { get; set; } = null!;
        public virtual AppUser AppUserLiked { get; set; } = null!;
    }
}
