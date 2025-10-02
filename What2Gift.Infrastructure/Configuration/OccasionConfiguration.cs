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

        builder.Property(o => o.StartMonth)
            .IsRequired();

        builder.Property(o => o.StartDay)
            .IsRequired();
        
        builder.Property(o => o.EndMonth)
            .IsRequired();
        
        builder.Property(o => o.EndDay)
            .IsRequired();
        
        builder.HasMany(o => o.GiftSuggestions)
            .WithOne(gs => gs.Occasion)
            .HasForeignKey(gs => gs.OccasionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(o => o.Products)
            .WithOne(gs => gs.Occasion)
            .HasForeignKey(gs => gs.OccasionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new Occasion
            {
                Id = Guid.Parse("99999999-1111-1111-1111-111111111111"),
                Name = "Birthday",
                Description = "Perfect gifts for birthdays",
                StartMonth = 1, StartDay = 1,
                EndMonth = 12, EndDay = 31
            },
            new Occasion
            {
                Id = Guid.Parse("88888888-2222-2222-2222-222222222222"),
                Name = "Christmas",
                Description = "Warm gifts for Christmas holidays",
                StartMonth = 12, StartDay = 20,
                EndMonth = 12, EndDay = 31
            },
            new Occasion
            {
                Id = Guid.Parse("77777777-3333-3333-3333-333333333333"),
                Name = "Valentine",
                Description = "Romantic gifts for Valentine's Day",
                StartMonth = 2, StartDay = 14,
                EndMonth = 2, EndDay = 14
            },
            new Occasion
            {
                Id = Guid.Parse("66666666-4444-4444-4444-444444444444"),
                Name = "Mother's Day",
                Description = "Special gifts to celebrate moms",
                StartMonth = 5, StartDay = 10,
                EndMonth = 5, EndDay = 12
            }
        );
    }
}