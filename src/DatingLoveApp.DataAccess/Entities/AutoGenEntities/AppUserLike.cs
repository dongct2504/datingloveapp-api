using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatingLoveApp.DataAccess.Entities
{
    [Table("AppUserLike")]
    public partial class AppUserLike
    {
        [Key]
        [StringLength(450)]
        [Unicode(false)]
        public string AppUserSourceId { get; set; } = null!;
        [Key]
        [StringLength(450)]
        [Unicode(false)]
        public string AppUserLikedId { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
    }
}
