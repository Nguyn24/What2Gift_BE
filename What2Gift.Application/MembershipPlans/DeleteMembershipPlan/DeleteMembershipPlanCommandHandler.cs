using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.MembershipPlans.DeleteMembershipPlan;

public class DeleteMembershipPlanCommandHandler(IDbContext context) : ICommandHandler<DeleteMembershipPlanCommand>
{
    public async Task<Result> Handle(DeleteMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var membershipPlan = await context.MembershipPlans
            .FirstOrDefaultAsync(mp => mp.Id == request.Id, cancellationToken);

        if (membershipPlan is null)
        {
            return Result.Failure<Guid>(Error.NotFound("MembershipPlan.NotFound", "Membership plan not found"));
        }

        context.MembershipPlans.Remove(membershipPlan);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
