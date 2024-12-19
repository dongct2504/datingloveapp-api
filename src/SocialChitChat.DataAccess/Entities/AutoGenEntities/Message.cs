using SocialChitChat.DataAccess.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities
{
    public partial class Message
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GroupChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime MessageSent { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateRead { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        public virtual GroupChat Conversation { get; set; } = null!;
        [ForeignKey("SenderId")]
        public virtual AppUser AppUser { get; set; } = null!;
    }
}
