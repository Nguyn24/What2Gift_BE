using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Finance;

namespace What2Gift.Infrastructure.Configuration;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.SpentAt)
            .IsRequired();
    }
}