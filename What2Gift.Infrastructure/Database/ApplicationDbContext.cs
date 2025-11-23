using MediatR;
using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Domain.Affiliate;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Products;
using What2Gift.Domain.Users;

namespace What2Gift.Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IPublisher publisher)
    : DbContext(options), IDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Occasion> Occasions => Set<Occasion>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductSource> ProductSources => Set<ProductSource>();
    public DbSet<GiftSuggestion> GiftSuggestions => Set<GiftSuggestion>();
    public DbSet<AffiliateClick> AffiliateClicks => Set<AffiliateClick>();
    public DbSet<IncomeSource> IncomeSources => Set<IncomeSource>();
    public DbSet<Revenue> Revenues => Set<Revenue>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<TopUpTransaction> TopUpTransactions => Set<TopUpTransaction>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var events = entity.DomainEvents;
                entity.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}