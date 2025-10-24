using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Memberships.GetAllMemberships;

public class GetAllMembershipsQuery : IPageableQuery, IQuery<Page<AdminMembershipResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SearchTerm { get; init; }
    public bool? IsActive { get; init; }
    public Guid? MembershipPlanId { get; init; }
    public DateTime? StartDateFrom { get; init; }
    public DateTime? StartDateTo { get; init; }
}

public class AdminMembershipResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public Guid MembershipPlanId { get; init; }
    public string MembershipPlanName { get; init; } = string.Empty;
    public decimal MembershipPlanPrice { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public bool IsActive { get; init; }
    public int DaysRemaining { get; init; }
}
