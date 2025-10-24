
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Finance;

namespace What2Gift.Infrastructure.Configuration;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.PaymentMethod)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.TransactionCode)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        // n-1: User ↔ PaymentTransaction
        builder.HasOne(t => t.User)
            .WithMany(u => u.PaymentTransactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // n-1: MembershipPlan ↔ PaymentTransaction
        builder.HasOne(t => t.MembershipPlan)
            .WithMany(p => p.PaymentTransactions)
            .HasForeignKey(t => t.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}