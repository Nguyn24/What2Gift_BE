using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Users;

namespace What2Gift.Infrastructure.Configuration;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(x => x.User)
            .WithMany(x => x.Feedbacks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Suggestion)
            .WithMany(x => x.Feedbacks)
            .HasForeignKey(x => x.SuggestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}