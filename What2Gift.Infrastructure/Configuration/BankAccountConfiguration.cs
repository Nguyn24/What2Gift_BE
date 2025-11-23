using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Finance;

namespace What2Gift.Infrastructure.Configuration;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BankName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.AccountNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.AccountHolderName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(b => b.QrCodeUrl)
            .HasMaxLength(1000);

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedAt)
            .IsRequired();
    }
}

