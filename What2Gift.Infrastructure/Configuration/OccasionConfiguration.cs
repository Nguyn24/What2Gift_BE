using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Configuration;

public class OccasionConfiguration : IEntityTypeConfiguration<Occasion>
{
    public void Configure(EntityTypeBuilder<Occasion> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .IsRequired();

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(o => o.Description)
            .HasMaxLength(1000);

        builder.Property(o => o.DateRangeStart)
            .IsRequired();

        builder.Property(o => o.DateRangeEnd)
            .IsRequired();
        
        builder.HasMany(o => o.GiftSuggestions)
            .WithOne(gs => gs.Occasion)
            .HasForeignKey(gs => gs.OccasionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(o => o.Products)
            .WithOne(gs => gs.Occasion)
            .HasForeignKey(gs => gs.OccasionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}