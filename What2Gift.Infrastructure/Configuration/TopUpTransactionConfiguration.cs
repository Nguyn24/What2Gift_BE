using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Finance;

namespace What2Gift.Infrastructure.Configuration;

public class TopUpTransactionConfiguration : IEntityTypeConfiguration<TopUpTransaction>
{
    public void Configure(EntityTypeBuilder<TopUpTransaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TransferContent)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.Points)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.Note)
            .HasMaxLength(1000);

        // n-1: User ↔ TopUpTransaction
        builder.HasOne(t => t.User)
            .WithMany(u => u.TopUpTransactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // n-1: User (Approver) ↔ TopUpTransaction
        builder.HasOne(t => t.Approver)
            .WithMany()
            .HasForeignKey(t => t.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

