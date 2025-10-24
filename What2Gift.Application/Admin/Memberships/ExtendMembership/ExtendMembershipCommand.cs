using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Memberships.ExtendMembership;

public class ExtendMembershipCommand : ICommand
{
    public Guid MembershipId { get; init; }
    public int AdditionalDays { get; init; }
    public string? Reason { get; init; }
}
