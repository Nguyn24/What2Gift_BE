using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.MembershipPlans.GetAllMembershipPlans;

public class GetAllMembershipPlansQuery : IPageableQuery, IQuery<Page<MembershipPlanResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public class MembershipPlanResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string? Description { get; init; }
}
