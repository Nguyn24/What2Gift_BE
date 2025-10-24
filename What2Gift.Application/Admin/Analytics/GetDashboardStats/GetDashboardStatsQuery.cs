using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Analytics.GetDashboardStats;

public class GetDashboardStatsQuery : IQuery<DashboardStatsResponse>
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}

public class DashboardStatsResponse
{
    public UserStats UserStats { get; init; } = new();
    public MembershipStats MembershipStats { get; init; } = new();
    public PaymentStats PaymentStats { get; init; } = new();
    public RevenueStats RevenueStats { get; init; } = new();
    public List<MonthlyRevenue> MonthlyRevenues { get; init; } = new();
    public List<MembershipPlanStats> MembershipPlanStats { get; init; } = new();
}

public class UserStats
{
    public int TotalUsers { get; init; }
    public int ActiveUsers { get; init; }
    public int NewUsersThisMonth { get; init; }
    public int NewUsersToday { get; init; }
    public double UserGrowthRate { get; init; }
}

public class MembershipStats
{
    public int TotalMemberships { get; init; }
    public int ActiveMemberships { get; init; }
    public int ExpiredMemberships { get; init; }
    public double MembershipConversionRate { get; init; }
    public int NewMembershipsThisMonth { get; init; }
}

public class PaymentStats
{
    public int TotalTransactions { get; init; }
    public int SuccessfulTransactions { get; init; }
    public int FailedTransactions { get; init; }
    public int PendingTransactions { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal SuccessAmount { get; init; }
    public double SuccessRate { get; init; }
}

public class RevenueStats
{
    public decimal TotalRevenue { get; init; }
    public decimal MonthlyRevenue { get; init; }
    public decimal TodayRevenue { get; init; }
    public double RevenueGrowthRate { get; init; }
    public decimal AverageOrderValue { get; init; }
}

public class MonthlyRevenue
{
    public string Month { get; init; } = string.Empty;
    public decimal Revenue { get; init; }
    public int TransactionCount { get; init; }
}

public class MembershipPlanStats
{
    public Guid PlanId { get; init; }
    public string PlanName { get; init; } = string.Empty;
    public decimal PlanPrice { get; init; }
    public int TotalSales { get; init; }
    public decimal TotalRevenue { get; init; }
    public int ActiveMemberships { get; init; }
}
