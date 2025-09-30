using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Finance;

namespace What2Gift.Infrastructure.Configuration;

public class RevenueConfiguration : IEntityTypeConfiguration<Revenue>
{
    public void Configure(EntityTypeBuilder<Revenue> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .IsRequired();

        builder.Property(r => r.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(r => r.Description)
            .HasMaxLength(1000);

        builder.Property(r => r.ReceivedAt)
            .IsRequired();

        builder.HasOne(r => r.IncomeSource)
            .WithMany(i => i.Revenues)
            .HasForeignKey(r => r.IncomeSourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}