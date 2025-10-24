using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.MembershipPlans.DeleteMembershipPlan;

public class DeleteMembershipPlanCommand : ICommand
{
    public Guid Id { get; init; }
}
