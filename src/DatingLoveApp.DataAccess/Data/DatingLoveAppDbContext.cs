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

        public virtual DbSet<LocalUser> LocalUsers { get; set; } = null!;
        public virtual DbSet<Picture> Pictures { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalUser>(entity =>
            {
                entity.Property(e => e.LocalUserId).ValueGeneratedNever();

                entity.Property(e => e.PhoneNumber).IsFixedLength();
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.PictureId).ValueGeneratedNever();

                entity.HasOne(d => d.LocalUser)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.LocalUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PICTURE_PICTURELO_LOCALUSER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
