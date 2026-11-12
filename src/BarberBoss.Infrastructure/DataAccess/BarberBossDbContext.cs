using BarberBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess;

public class BarberBossDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Billing> Billings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billing>()
            .ToTable("billings");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Id = Guid.NewGuid();
                entry.Entity.CreatedAt = now;
            }
            if (entry.State == EntityState.Modified)
                entry.Property(p => p.CreatedAt).IsModified = false;
            entry.Entity.UpdatedAt = now;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
