using SocialChitChat.DataAccess.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities;

public partial class Comment
{
    [Key]
    public Guid Id { get; set; }
    public Guid? ReplyId { get; set; }
    public Guid AppUserId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public AppUser User { get; set; } = null!;
    public Post Post { get; set; } = null!;

    public Comment? Reply { get; set; }
    public ICollection<Comment>? Replies { get; set; }
}
