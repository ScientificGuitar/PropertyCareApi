using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyCareApi.Models;

namespace PropertyCareApi.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("audit_logs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}