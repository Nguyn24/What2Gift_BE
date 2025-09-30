using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Affiliate;

namespace What2Gift.Infrastructure.Configuration;

public class AffiliateClickConfiguration : IEntityTypeConfiguration<AffiliateClick>
{
    public void Configure(EntityTypeBuilder<AffiliateClick> builder)
    {
        builder.HasKey(ac => ac.Id);

        builder.Property(ac => ac.Id)
            .IsRequired();

        builder.Property(ac => ac.ClickedAt)
            .IsRequired();

        builder.Property(ac => ac.UserAgent)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ac => ac.IpAddress)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(ac => ac.User)
            .WithMany(u => u.AffiliateClicks)
            .HasForeignKey(ac => ac.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ac => ac.ProductSource)
            .WithMany(ps => ps.AffiliateClicks)
            .HasForeignKey(ac => ac.ProductSourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}