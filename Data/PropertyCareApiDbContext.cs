using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyCareApi.Models;

namespace PropertyCareApi.Data
{
    public class PropertyCareApiDbContext : DbContext
    {
        public PropertyCareApiDbContext(DbContextOptions<PropertyCareApiDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropertyCareApiDbContext).Assembly);
        }
    }
}