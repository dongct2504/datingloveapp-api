using SocialChitChat.DataAccess.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities;

public partial class Like
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public AppUser User { get; set; } = null!;
    public Post Post { get; set; } = null!;
}
