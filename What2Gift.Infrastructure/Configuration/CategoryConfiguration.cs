using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(b => b.Products)
            .WithOne(gs => gs.Category)
            .HasForeignKey(gs => gs.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new Category
            {
                Id = Guid.Parse("aaaa1111-1111-1111-1111-111111111111"),
                Name = "Flowers"
            },
            new Category
            {
                Id = Guid.Parse("bbbb2222-2222-2222-2222-222222222222"),
                Name = "Handmade Crafts"
            },
            new Category
            {
                Id = Guid.Parse("cccc3333-3333-3333-3333-333333333333"),
                Name = "Food & Sweets"
            },
            new Category
            {
                Id = Guid.Parse("dddd4444-4444-4444-4444-444444444444"),
                Name = "Accessories"
            }
        );

    }
}