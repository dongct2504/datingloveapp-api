using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities
{
    public partial class Message
    {
        [Key]
        public Guid MessageId { get; set; }
        [StringLength(450)]
        [Unicode(false)]
        public string SenderId { get; set; } = null!;
        [StringLength(256)]
        public string SenderUserName { get; set; } = null!;
        [StringLength(450)]
        [Unicode(false)]
        public string RecipientId { get; set; } = null!;
        [StringLength(256)]
        public string RecipientUserName { get; set; } = null!;
        [StringLength(1024)]
        public string Content { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime MessageSent { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateRead { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
