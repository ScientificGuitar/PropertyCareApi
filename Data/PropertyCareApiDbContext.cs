using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PropertyCareApi.Models;

namespace PropertyCareApi.Data
{
    public class PropertyCareApiDbContext(DbContextOptions<PropertyCareApiDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropertyCareApiDbContext).Assembly);

            // https://www.milanjovanovic.tech/blog/implementing-soft-delete-with-ef-core
            // What this basically does is append `WHERE DeletedAt IS NULL` to most queries.
            modelBuilder.Entity<User>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<Property>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<MaintenanceRequest>().HasQueryFilter(r => !r.IsDeleted);

            ApplyBaseEntityConfiguration(modelBuilder);
        }

        // https://www.j-labs.pl/en/tech-blog/who-modified-it-and-when-a-clean-way-to-update-audit-columns-in-a-sql-database/
        // Updates the audit fields for models inheriting BaseEntity when added/updated
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public static void ApplyBaseEntityConfiguration(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                    continue;

                var entity = modelBuilder.Entity(entityType.ClrType);

                entity.Ignore(nameof(BaseEntity.IsDeleted));

                entity.HasIndex(nameof(BaseEntity.DeletedAt));

                entity.Property(nameof(BaseEntity.CreatedAt))
                      .IsRequired();
            }
        }
    }
}