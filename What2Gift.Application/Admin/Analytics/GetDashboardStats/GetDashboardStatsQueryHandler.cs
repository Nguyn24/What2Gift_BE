using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.Analytics.GetDashboardStats;

public class GetDashboardStatsQueryHandler(IDbContext context) : IQueryHandler<GetDashboardStatsQuery, DashboardStatsResponse>
{
    public async Task<Result<DashboardStatsResponse>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        DateTime ToUtc(DateTime dt) => dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };

        var nowUtc = DateTime.UtcNow;
        var fromDate = request.FromDate.HasValue ? ToUtc(request.FromDate.Value) : nowUtc.AddMonths(-12);
        var toDate = request.ToDate.HasValue ? ToUtc(request.ToDate.Value) : nowUtc;
        var thisMonthStart = new DateTime(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var today = DateTime.SpecifyKind(nowUtc.Date, DateTimeKind.Utc);

        // User Stats
        var totalUsers = await context.Users.CountAsync(cancellationToken);
        var activeUsers = await context.Users.CountAsync(u => u.Status == Domain.Users.UserStatus.Active, cancellationToken);
        var newUsersThisMonth = await context.Users.CountAsync(u => u.CreatedAt >= thisMonthStart, cancellationToken);
        var newUsersToday = await context.Users.CountAsync(u => u.CreatedAt >= today, cancellationToken);
        
        var lastMonthUsers = await context.Users.CountAsync(u => u.CreatedAt < thisMonthStart, cancellationToken);
        var userGrowthRate = lastMonthUsers > 0 ? ((double)(totalUsers - lastMonthUsers) / lastMonthUsers) * 100 : 0;

        // Membership Stats
        var totalMemberships = await context.Memberships.CountAsync(cancellationToken);
        var activeMemberships = await context.Memberships.CountAsync(m => m.EndDate > DateOnly.FromDateTime(nowUtc), cancellationToken);
        var expiredMemberships = totalMemberships - activeMemberships;
        var newMembershipsThisMonth = await context.Memberships.CountAsync(m => m.StartDate >= DateOnly.FromDateTime(thisMonthStart), cancellationToken);
        var membershipConversionRate = totalUsers > 0 ? ((double)totalMemberships / totalUsers) * 100 : 0;

        // Payment Stats
        var totalTransactions = await context.PaymentTransactions.CountAsync(cancellationToken);
        var successfulTransactions = await context.PaymentTransactions.CountAsync(pt => pt.Status == PaymentTransactionStatus.Success, cancellationToken);
        var failedTransactions = await context.PaymentTransactions.CountAsync(pt => pt.Status == PaymentTransactionStatus.Failed, cancellationToken);
        var pendingTransactions = await context.PaymentTransactions.CountAsync(pt => pt.Status == PaymentTransactionStatus.Pending, cancellationToken);
        
        var totalAmount = await context.PaymentTransactions.SumAsync(pt => pt.Amount, cancellationToken);
        var successAmount = await context.PaymentTransactions
            .Where(pt => pt.Status == PaymentTransactionStatus.Success)
            .SumAsync(pt => pt.Amount, cancellationToken);
        var successRate = totalTransactions > 0 ? ((double)successfulTransactions / totalTransactions) * 100 : 0;

        // Revenue Stats
        var totalRevenue = successAmount;
        var monthlyRevenue = await context.PaymentTransactions
            .Where(pt => pt.Status == PaymentTransactionStatus.Success && pt.PaidAt >= thisMonthStart)
            .SumAsync(pt => pt.Amount, cancellationToken);
        var todayRevenue = await context.PaymentTransactions
            .Where(pt => pt.Status == PaymentTransactionStatus.Success && pt.PaidAt >= today)
            .SumAsync(pt => pt.Amount, cancellationToken);
        
        var lastMonthRevenue = await context.PaymentTransactions
            .Where(pt => pt.Status == PaymentTransactionStatus.Success && pt.PaidAt < thisMonthStart)
            .SumAsync(pt => pt.Amount, cancellationToken);
        var revenueGrowthRate = lastMonthRevenue > 0 ? ((double)(monthlyRevenue - lastMonthRevenue) / (double)lastMonthRevenue) * 100 : 0;
        var averageOrderValue = successfulTransactions > 0 ? successAmount / successfulTransactions : 0;

        // Monthly Revenue Chart Data
        var monthlyRevenueRows = await context.PaymentTransactions
            .Where(pt => pt.Status == PaymentTransactionStatus.Success && pt.PaidAt >= fromDate && pt.PaidAt <= toDate)
            .GroupBy(pt => new { Year = pt.PaidAt!.Value.Year, Month = pt.PaidAt.Value.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Revenue = g.Sum(pt => pt.Amount),
                TransactionCount = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync(cancellationToken);

        var monthlyRevenues = monthlyRevenueRows
            .Select(x => new MonthlyRevenue
            {
                Month = $"{x.Year}-{x.Month:D2}",
                Revenue = x.Revenue,
                TransactionCount = x.TransactionCount
            })
            .ToList();

        // Membership Plan Stats
        var membershipPlanStats = await context.MembershipPlans
            .Select(mp => new MembershipPlanStats
            {
                PlanId = mp.Id,
                PlanName = mp.Name,
                PlanPrice = mp.Price,
                TotalSales = mp.PaymentTransactions.Count(pt => pt.Status == PaymentTransactionStatus.Success),
                TotalRevenue = mp.PaymentTransactions
                    .Where(pt => pt.Status == PaymentTransactionStatus.Success)
                    .Sum(pt => pt.Amount),
                ActiveMemberships = context.Memberships.Count(m => m.MembershipPlanId == mp.Id && m.EndDate > DateOnly.FromDateTime(DateTime.Now))
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new DashboardStatsResponse
        {
            UserStats = new UserStats
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                NewUsersThisMonth = newUsersThisMonth,
                NewUsersToday = newUsersToday,
                UserGrowthRate = userGrowthRate
            },
            MembershipStats = new MembershipStats
            {
                TotalMemberships = totalMemberships,
                ActiveMemberships = activeMemberships,
                ExpiredMemberships = expiredMemberships,
                MembershipConversionRate = membershipConversionRate,
                NewMembershipsThisMonth = newMembershipsThisMonth
            },
            PaymentStats = new PaymentStats
            {
                TotalTransactions = totalTransactions,
                SuccessfulTransactions = successfulTransactions,
                FailedTransactions = failedTransactions,
                PendingTransactions = pendingTransactions,
                TotalAmount = totalAmount,
                SuccessAmount = successAmount,
                SuccessRate = successRate
            },
            RevenueStats = new RevenueStats
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                TodayRevenue = todayRevenue,
                RevenueGrowthRate = revenueGrowthRate,
                AverageOrderValue = averageOrderValue
            },
            MonthlyRevenues = monthlyRevenues,
            MembershipPlanStats = membershipPlanStats
        });
    }
}
