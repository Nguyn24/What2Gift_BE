using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Users;

namespace What2Gift.Infrastructure.Configuration;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();
        
        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        // Relationship
        builder.HasOne(m => m.User)
            .WithOne(u => u.Membership)
            .HasForeignKey<Membership>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(m => m.MembershipPlan)
            .WithMany()
            .HasForeignKey(m => m.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}