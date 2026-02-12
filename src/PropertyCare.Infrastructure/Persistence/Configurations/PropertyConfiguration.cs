using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyCare.Domain.Entities;

namespace PropertyCare.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("properties");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasOne(p => p.Owner)
            .WithMany(u => u.Properties)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
