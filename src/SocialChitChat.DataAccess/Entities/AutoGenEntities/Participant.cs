using SocialChitChat.DataAccess.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities
{
    public partial class Participant
    {
        [Key]
        public Guid ConversationId { get; set; }
        [Key]
        public Guid AppUserId { get; set; }
        public bool IsAdmin { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime JoinAt { get; set; }

        public virtual GroupChat Conversation { get; set; } = null!;
        public virtual AppUser AppUser { get; set; } = null!;
    }
}
