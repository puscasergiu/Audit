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

                entity.Property(e => e.NewValues);

                entity.Property(e => e.OldValues);

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<AuditEntry> auditEntries = AuditService.ProcessChanges(ChangeTracker.Entries());

            int result = await base.SaveChangesAsync(cancellationToken);

            AuditService.ProcessChangesAfterSave(auditEntries);

            await HandleLogsAsync(auditEntries);

            return result;
        }

        private async Task HandleLogsAsync(List<AuditEntry> entries)
        {
            List<AuditLog> logs = new List<AuditLog>();

            foreach (AuditEntry entry in entries)
            {
                logs.Add(entry.ToAuditLog());
            }

            AuditLog.AddRange(logs);

            await base.SaveChangesAsync();
        }
    }
}
