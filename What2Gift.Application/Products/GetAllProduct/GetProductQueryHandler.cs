using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Products.GetAllProduct;

public class GetProductQueryHandler(IDbContext context)
    : IQueryHandler<GetProductQuery, Page<GetProductResponse>>
{
    public async Task<Result<Page<GetProductResponse>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var query = context.Products;
        var totalCount = await query.CountAsync(cancellationToken);
        
        var result = await query
            .Include(s=>s.ProductSources)
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(p=> new GetProductResponse
            {
                Id = p.Id,
                BrandId = p.BrandId,
                OccasionId = p.OccasionId,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                ProductSources = p.ProductSources.Select(ps => new ProductSources
                {
                    Id = ps.Id,
                    VendorName = ps.VendorName,
                    Price = ps.Price,
                    AffiliateLink = ps.AffiliateLink
                }).ToList()
            }) .ToListAsync(cancellationToken);
        
        return new Page<GetProductResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}