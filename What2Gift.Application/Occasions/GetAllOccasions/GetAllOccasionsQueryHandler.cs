using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Occasions.GetAllOccasions;

public class GetAllOccasionsQueryHandler(IDbContext context) : IQueryHandler<GetAllOccasionsQuery, Page<OccasionResponse>>
{
    public async Task<Result<Page<OccasionResponse>>> Handle(GetAllOccasionsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Occasions.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(o => new OccasionResponse
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                StartMonth = o.StartMonth,
                StartDay = o.StartDay,
                EndMonth = o.EndMonth,
                EndDay = o.EndDay
            })
            .ToListAsync(cancellationToken);

        return new Page<OccasionResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
