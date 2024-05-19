using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatingLoveApp.DataAccess.Entities
{
    [Index("LocalUserId", Name = "PICTURESLOCALUSERS_FK")]
    public partial class Picture
    {
        [Key]
        public Guid PictureId { get; set; }
        public Guid? LocalUserId { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string ImageUrl { get; set; } = null!;
        public bool IsMain { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string PublicId { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("LocalUserId")]
        [InverseProperty("Pictures")]
        public virtual LocalUser? LocalUser { get; set; }
    }
}
