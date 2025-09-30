using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Configuration;

public class GiftSuggestionConfiguration : IEntityTypeConfiguration<GiftSuggestion>
{
    public void Configure(EntityTypeBuilder<GiftSuggestion> builder)
    {
        builder.HasKey(gs => gs.Id);

        builder.Property(gs => gs.Id)
            .IsRequired();

        builder.Property(gs => gs.RecipientGender)
            .IsRequired();

        builder.Property(gs => gs.RecipientAge)
            .IsRequired();

        builder.Property(gs => gs.RecipientHobby)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(gs => gs.BudgetMin)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(gs => gs.BudgetMax)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(gs => gs.CreatedAt)
            .IsRequired();

        builder.HasOne(gs => gs.User)
            .WithMany(u => u.GiftSuggestions)
            .HasForeignKey(gs => gs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gs => gs.Occasion)
            .WithMany(o => o.GiftSuggestions)
            .HasForeignKey(gs => gs.OccasionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(gs => gs.SuggestedProduct)
            .WithMany(p => p.GiftSuggestions)
            .HasForeignKey(gs => gs.SuggestedProductId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(gs => gs.Feedbacks)
            .WithOne(f => f.Suggestion)
            .HasForeignKey(f => f.SuggestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}