using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.MembershipPlans.UpdateMembershipPlan;

public class UpdateMembershipPlanCommandHandler(IDbContext context) : ICommandHandler<UpdateMembershipPlanCommand>
{
    public async Task<Result> Handle(UpdateMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var membershipPlan = await context.MembershipPlans
            .FirstOrDefaultAsync(mp => mp.Id == request.Id, cancellationToken);

        if (membershipPlan is null)
        {
            return Result.Failure<Guid>(Error.NotFound("MembershipPlan.NotFound", "Membership plan not found"));
        }

        membershipPlan.Name = request.Name;
        membershipPlan.Price = request.Price;
        membershipPlan.Description = request.Description;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
