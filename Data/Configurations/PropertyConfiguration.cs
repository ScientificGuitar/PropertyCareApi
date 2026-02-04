using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyCareApi.Models;

namespace PropertyCareApi.Data.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("properties");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.HasOne(p => p.Owner)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}