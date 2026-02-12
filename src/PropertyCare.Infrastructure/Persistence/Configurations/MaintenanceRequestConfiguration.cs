using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyCare.Domain.Entities;

namespace PropertyCare.Infrastructure.Persistence.Configurations;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
    {
        builder.ToTable("maintenance_requests");

        builder.Property(m => m.Category)
           .IsRequired();

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(m => m.Priority)
            .IsRequired();

        builder.Property(m => m.Status)
            .IsRequired();

        builder.HasOne(m => m.Property)
            .WithMany(p => p.MaintenanceRequests)
            .HasForeignKey(m => m.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
