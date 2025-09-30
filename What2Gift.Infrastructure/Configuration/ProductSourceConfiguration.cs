using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Configuration;

public class ProductSourceConfiguration : IEntityTypeConfiguration<ProductSource>
{
    public void Configure(EntityTypeBuilder<ProductSource> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
            .IsRequired();

        builder.Property(ps => ps.VendorName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ps => ps.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ps => ps.AffiliateLink)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(ps => ps.Product)
            .WithMany(p => p.ProductSources)
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ps => ps.AffiliateClicks)
            .WithOne(ac => ac.ProductSource)
            .HasForeignKey(ac => ac.ProductSourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}