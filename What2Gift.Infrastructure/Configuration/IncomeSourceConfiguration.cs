using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Finance;

namespace What2Gift.Infrastructure.Configuration;

public class IncomeSourceConfiguration : IEntityTypeConfiguration<IncomeSource>
{
    public void Configure(EntityTypeBuilder<IncomeSource> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .IsRequired();

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.Description)
            .HasMaxLength(1000);

        builder.HasMany(i => i.Revenues)
            .WithOne(r => r.IncomeSource)
            .HasForeignKey(r => r.IncomeSourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}