using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AL.Infrastructure.Persistance.Models
{
    public partial class AuditLogDbContext : DbContext
    {
        public AuditLogDbContext()
        {
        }

        public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditTrail> AuditTrails { get; set; } = null!;
        public virtual DbSet<AuditType> AuditTypes { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.ToTable("AuditTrail");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.TableName).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.AuditTypeNavigation)
                    .WithMany(p => p.AuditTrails)
                    .HasForeignKey(d => d.AuditType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditTrail_AuditType");
            });

            modelBuilder.Entity<AuditType>(entity =>
            {
                entity.ToTable("AuditType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AuditType1)
                    .HasMaxLength(50)
                    .HasColumnName("AuditType");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUser).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
