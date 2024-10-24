using System;
using System.Collections.Generic;
using DatingLoveApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DatingLoveApp.DataAccess.Data
{
    public partial class DatingLoveAppDbContext : DbContext
    {
        public DatingLoveAppDbContext()
        {
        }

        public DatingLoveAppDbContext(DbContextOptions<DatingLoveAppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUserLike> AppUserLikes { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Picture> Pictures { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserLike>(entity =>
            {
                entity.HasKey(e => new { e.AppUserSourceId, e.AppUserLikedId })
                    .HasName("PK_APPUSERLIKE");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.PictureId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
