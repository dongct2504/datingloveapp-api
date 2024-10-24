using Microsoft.AspNetCore.Identity;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Identity;

public class AppUser : IdentityUser<Guid>
{
    [StringLength(30)]
    public string? FirstName { get; set; }

    [StringLength(30)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string Nickname { get; set; } = null!;

    public byte Gender { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    [StringLength(256)]
    public string? Introduction { get; set; }

    [StringLength(128)]
    public string? Interest { get; set; }

    [StringLength(128)]
    public string? IdealType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastActive { get; set; }

    [StringLength(128)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Ward { get; set; }

    [StringLength(30)]
    public string? District { get; set; }

    [StringLength(30)]
    public string? City { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();

    public ICollection<AppUserLike> AppUserLikes { get; set; } = new List<AppUserLike>();
    public ICollection<AppUserLike> LikedByUsers { get; set; } = new List<AppUserLike>();

    public ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}
