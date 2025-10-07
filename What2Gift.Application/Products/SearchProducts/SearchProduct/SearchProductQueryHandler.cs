using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Products.SearchProducts.SearchProduct;

public class SearchProductQueryHandler(IDbContext context)
    : IQueryHandler<SearchProductQuery, Page<GetProductResponse>>
{
    public async Task<Result<Page<GetProductResponse>>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
    {
        var query = context.Products
            .Include(s => s.ProductSources)
            .AsQueryable();

        // Apply filters based on provided parameters
        if (request.BrandId.HasValue)
        {
            query = query.Where(p => p.BrandId == request.BrandId.Value);
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        if (request.OccasionId.HasValue)
        {
            query = query.Where(p => p.OccasionId == request.OccasionId.Value);
        }

        if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.ProductSources.Any(ps => 
                (!request.MinPrice.HasValue || ps.Price >= request.MinPrice.Value) &&
                (!request.MaxPrice.HasValue || ps.Price <= request.MaxPrice.Value)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(p => new GetProductResponse
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
            })
            .ToListAsync(cancellationToken);
        
        return new Page<GetProductResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
