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

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.HasMany(b => b.Products)
            .WithOne(gs => gs.Category)
            .HasForeignKey(gs => gs.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}