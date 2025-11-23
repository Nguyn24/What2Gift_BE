using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Memberships.RegisterMembership;

public class RegisterMembershipCommand : ICommand<RegisterMembershipResponse>
{
    public Guid UserId { get; init; }
    public Guid MembershipPlanId { get; init; }
}

public class RegisterMembershipResponse
{
    public Guid MembershipId { get; init; }
    public int PointsUsed { get; init; }
    public int RemainingPoints { get; init; }
}
