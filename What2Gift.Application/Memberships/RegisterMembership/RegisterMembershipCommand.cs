using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Memberships.RegisterMembership;

public class RegisterMembershipCommand : ICommand<RegisterMembershipResponse>
{
    public Guid UserId { get; init; }
    public Guid MembershipPlanId { get; init; }
    public string ReturnUrl { get; init; } = string.Empty;
}

public class RegisterMembershipResponse
{
    public string PaymentUrl { get; init; } = string.Empty;
    public Guid PaymentTransactionId { get; init; }
}
