using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Brands.GetAllBrands;

public class GetAllBrandsQueryHandler(IDbContext context) : IQueryHandler<GetAllBrandsQuery, Page<BrandResponse>>
{
    public async Task<Result<Page<BrandResponse>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Brands.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(b => new BrandResponse
            {
                Id = b.Id,
                Name = b.Name
            })
            .ToListAsync(cancellationToken);

        return new Page<BrandResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
