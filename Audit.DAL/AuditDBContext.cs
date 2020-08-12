using System.Threading;
using System.Threading.Tasks;

using Audit.DAL.Models;

using Microsoft.EntityFrameworkCore;

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
                entity.Property(e => e.ColumnName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.NewValue).IsRequired();

                entity.Property(e => e.OldValue).IsRequired();

                entity.Property(e => e.PrimaryKey)
                    .IsRequired()
                    .HasMaxLength(50);

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
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
