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

        builder.HasMany(b => b.Products)
            .WithOne(gs => gs.Brand)
            .HasForeignKey(gs => gs.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new Brand
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Handmade Corner"
            },
            new Brand
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Bloom & Co"
            },
            new Brand
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Sweet Delights"
            },
            new Brand
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Giftopia"
            }
        );

    }
}