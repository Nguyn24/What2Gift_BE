using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Products.SearchProducts.SearchProductByBudget;

public class GetProductByBudgetQueryHandler(IDbContext context)
    : IQueryHandler<GetProductByBudgetQuery, Page<GetProductResponse>>
{
    public async Task<Result<Page<GetProductResponse>>> Handle(GetProductByBudgetQuery request, CancellationToken cancellationToken)
    {
        var query = context.Products
            .Include(s=>s.ProductSources)
            .Where(p => p.ProductSources.Any(ps => ps.Price >= request.MinPrice && ps.Price <= request.MaxPrice));
        var totalCount = await query.CountAsync(cancellationToken);
        
        var result = await query
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