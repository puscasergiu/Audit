using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Audit.DAL.Infrastructure;
using Audit.DAL.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audit.DAL
{
    public class AuditDBContext : DbContext
    {
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }

        public AuditDBContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.KeyValues).IsRequired();

                entity.Property(e => e.NewValues).IsRequired();

                entity.Property(e => e.OldValues).IsRequired();

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }

        public override int SaveChanges()
        {
            AuditService.ProcessChanges(ChangeTracker.Entries());

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<AuditEntry> auditEntries = AuditService.ProcessChanges(ChangeTracker.Entries());

            Task<int> result = base.SaveChangesAsync(cancellationToken);

            AuditService.AfterProcessChanges(auditEntries);

            List<AuditLog> logs = new List<AuditLog>();

            foreach (var item in auditEntries)
            {
                logs.Add(item.ToAudit());
            }

            return result;
        }
    }
}
