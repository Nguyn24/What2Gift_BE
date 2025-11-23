using Microsoft.EntityFrameworkCore;
using What2Gift.Domain.Affiliate;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Products;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Abstraction.Data;

public interface IDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Membership> Memberships { get; }
    DbSet<Brand> Brands { get; }
    DbSet<Category> Categories { get; }
    DbSet<Occasion> Occasions { get; }
    DbSet<Product> Products { get; }
    DbSet<ProductSource> ProductSources { get; }
    DbSet<GiftSuggestion> GiftSuggestions { get; }
    DbSet<AffiliateClick> AffiliateClicks { get; }
    DbSet<IncomeSource> IncomeSources { get; }
    DbSet<Revenue> Revenues { get; }
    DbSet<Expense> Expenses { get; }
    DbSet<Feedback> Feedbacks { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<MembershipPlan> MembershipPlans { get; }
    DbSet<PaymentTransaction> PaymentTransactions { get; }
    DbSet<TopUpTransaction> TopUpTransactions { get; }
    DbSet<BankAccount> BankAccounts { get; }

    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}