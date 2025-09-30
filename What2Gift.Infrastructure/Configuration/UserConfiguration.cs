using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.Users;

namespace What2Gift.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.Username)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Role)
                .HasConversion<string>()   
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()   
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.MembershipStatus)
                .HasConversion<string>()   
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
            
            builder.Property(x => x.IsVerified).IsRequired().HasDefaultValue(false);
           
            builder.HasOne(u => u.Membership)          
                .WithOne(m => m.User)              
                .HasForeignKey<Membership>(m => m.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.GiftSuggestions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AffiliateClicks)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Feedbacks)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Notifications)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

    }
}