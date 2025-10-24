using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Users;

namespace What2Gift.Infrastructure.Configuration;

public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.HasMany(p => p.PaymentTransactions)
            .WithOne(t => t.MembershipPlan)
            .HasForeignKey(t => t.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new MembershipPlan
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Basic",
                Price = 20000,
                Description = "Gói Basic: quyền truy cập cơ bản trong 1 tháng"
            },
            new MembershipPlan
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Pro",
                Price = 40000,
                Description = "Gói Pro: đầy đủ quyền lợi trong 1 tháng"
            }
        );
    }
}