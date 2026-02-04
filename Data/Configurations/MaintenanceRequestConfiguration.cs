using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyCareApi.Models;

namespace PropertyCareApi.Data.Configurations
{
    public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
        {
            builder.ToTable("maintenance_requests");

            builder.Property(m => m.Category)
               .IsRequired()
               .HasMaxLength(128);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(m => m.Priority)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired();

            builder.Property(m => m.CreatedAt)
                .IsRequired();

            builder.HasOne(m => m.Property)
                .WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}