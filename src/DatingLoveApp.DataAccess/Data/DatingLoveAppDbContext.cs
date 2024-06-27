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

        public virtual DbSet<Picture> Pictures { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.PictureId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
