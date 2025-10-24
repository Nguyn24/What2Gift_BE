using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;

namespace What2Gift.Application.MembershipPlans.CreateMembershipPlan;

public class CreateMembershipPlanCommandHandler(IDbContext context) : ICommandHandler<CreateMembershipPlanCommand>
{
    public async Task<Result> Handle(CreateMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var membershipPlan = new MembershipPlan
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        context.MembershipPlans.Add(membershipPlan);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(membershipPlan.Id);
    }
}
