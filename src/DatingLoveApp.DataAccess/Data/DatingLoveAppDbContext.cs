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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
