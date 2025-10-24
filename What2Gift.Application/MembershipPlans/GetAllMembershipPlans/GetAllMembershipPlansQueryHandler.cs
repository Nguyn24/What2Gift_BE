using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.MembershipPlans.GetAllMembershipPlans;

public class GetAllMembershipPlansQueryHandler(IDbContext context) : IQueryHandler<GetAllMembershipPlansQuery, Page<MembershipPlanResponse>>
{
    public async Task<Result<Page<MembershipPlanResponse>>> Handle(GetAllMembershipPlansQuery request, CancellationToken cancellationToken)
    {
        var query = context.MembershipPlans.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(mp => new MembershipPlanResponse
            {
                Id = mp.Id,
                Name = mp.Name,
                Price = mp.Price,
                Description = mp.Description
            })
            .ToListAsync(cancellationToken);

        return new Page<MembershipPlanResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
