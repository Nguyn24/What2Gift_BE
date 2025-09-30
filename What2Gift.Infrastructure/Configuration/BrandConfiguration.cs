using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Configuration;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.HasMany(b => b.Products)
            .WithOne(gs => gs.Brand)
            .HasForeignKey(gs => gs.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}