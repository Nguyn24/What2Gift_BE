using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;
using What2Gift.Domain.Products.Errors;

namespace What2Gift.Application.Products.UpdateProduct;

public class UpdateProductCommandHandler(IDbContext context)
    : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .Include(p => p.ProductSources)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
            return Result.Failure<Guid>(ProductErrors.ProductNotFound);

        // Validate name
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure<Guid>(ProductErrors.NameRequired);

        // Validate Brand
        var brandExists = await context.Brands.AnyAsync(b => b.Id == request.BrandId, cancellationToken);
        if (!brandExists)
            return Result.Failure<Guid>(ProductErrors.BrandNotFound);

        // Validate Category
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
            return Result.Failure<Guid>(ProductErrors.CategoryNotFound);

        // Validate Occasion 
        if (request.OccasionId.HasValue)
        {
            var occasionExists = await context.Occasions
                .AnyAsync(o => o.Id == request.OccasionId.Value, cancellationToken);
            if (!occasionExists)
                return Result.Failure<Guid>(ProductErrors.OccasionNotFound);
        }

        // Validate ProductSources
        if (request.ProductSources == null || !request.ProductSources.Any())
            return Result.Failure<Guid>(ProductErrors.InvalidProductSource);

        if (request.ProductSources.Any(ps =>
                string.IsNullOrWhiteSpace(ps.VendorName) ||
                ps.Price <= 0 ||
                string.IsNullOrWhiteSpace(ps.AffiliateLink)))
        {
            return Result.Failure<Guid>(ProductErrors.InvalidProductSource);
        }

        product.BrandId = request.BrandId ?? product.BrandId;
        product.CategoryId = request.CategoryId ?? product.CategoryId;
        product.OccasionId = request.OccasionId ?? product.OccasionId;
        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.ImageUrl = request.ImageUrl ?? product.ImageUrl;

        var incomingIds = request.ProductSources.Where(ps => ps.Id.HasValue).Select(ps => ps.Id!.Value).ToList();
        var toRemove = product.ProductSources.Where(ps => !incomingIds.Contains(ps.Id)).ToList();
        foreach (var remove in toRemove)
        {
            product.ProductSources.Remove(remove);
        }

        foreach (var psRequest in request.ProductSources)
        {
            if (psRequest.Id.HasValue)
            {
                var existing = product.ProductSources.FirstOrDefault(ps => ps.Id == psRequest.Id.Value);
                if (existing != null)
                {
                    existing.VendorName = psRequest.VendorName;
                    existing.Price = psRequest.Price;
                    existing.AffiliateLink = psRequest.AffiliateLink;
                }
            }
            else
            {
                product.ProductSources.Add(new ProductSource
                {
                    Id = Guid.NewGuid(),
                    VendorName = psRequest.VendorName,
                    Price = psRequest.Price,
                    AffiliateLink = psRequest.AffiliateLink
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(product.Id);
    }
}