using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.AI;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Products.AISuggestion;

public class GetAiSuggestedProductsCommandHandler(
    IDbContext context,
    IGiftSuggestionAiService aiService,
    IUserContext userContext) : ICommandHandler<GetAiSuggestedProductsCommand, GetProductResponse>
{
    public async Task<Result<GetProductResponse>> Handle(GetAiSuggestedProductsCommand request, CancellationToken cancellationToken)
    {
        
        var giftSuggestion = new GiftSuggestion
        {
            UserId = userContext.UserId,
            OccasionId = request.OccasionId,
            RecipientGender = request.RecipientGender,
            RecipientAge = request.RecipientAge,
            RecipientHobby = request.RecipientHobby,
            BudgetMin = request.BudgetMin,
            BudgetMax = request.BudgetMax,
            CreatedAt = DateTime.UtcNow
        };
        
        var suggestedProducts = await aiService.GetAiSuggestedProductsAsync(giftSuggestion, cancellationToken);
        
        var productIds = suggestedProducts.Select(p => p.Id).ToList();
        
        if (!productIds.Any())
        {
            return new GetProductResponse(
              );
        }
        
        // Get products with sources (no pagination)
        var result = await context.Products
            .Include(p => p.ProductSources)
            .Where(p => productIds.Contains(p.Id))
            .OrderBy(p => p.Name) // Add OrderBy to fix warning
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
        
        return new GetProductResponse()
            ;
    }
}
