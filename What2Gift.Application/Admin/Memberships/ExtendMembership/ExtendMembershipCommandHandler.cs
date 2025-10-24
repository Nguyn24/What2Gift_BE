using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Memberships.ExtendMembership;

public class ExtendMembershipCommandHandler(IDbContext context) : ICommandHandler<ExtendMembershipCommand>
{
    public async Task<Result> Handle(ExtendMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await context.Memberships
            .FirstOrDefaultAsync(m => m.Id == request.MembershipId, cancellationToken);

        if (membership is null)
        {
            return Result.Failure(Error.NotFound("Membership.NotFound", "Membership not found"));
        }

        // Extend membership by adding days
        membership.EndDate = membership.EndDate.AddDays(request.AdditionalDays);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
