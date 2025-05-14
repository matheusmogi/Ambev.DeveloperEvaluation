using Ambev.DeveloperEvaluation.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.CustomerId).IsRequired().HasColumnType("int");
        builder.Property(s => s.SaleDate).IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(s => s.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(s => s.UpdatedAt).HasColumnType("timestamp with time zone");

        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey(si => si.SaleId);
    }
}
