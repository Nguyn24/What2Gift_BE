using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;
using What2Gift.Domain.Products.Errors;

namespace What2Gift.Application.Products.CreateProduct;

public class CreateProductCommandHandler(IDbContext context)
: ICommandHandler<CreateProductCommand>
{
    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure(ProductErrors.NameRequired);

        // Validate Brand
        var brandExists = await context.Brands
            .AnyAsync(b => b.Id == request.BrandId, cancellationToken);
        if (!brandExists)
            return Result.Failure(ProductErrors.BrandNotFound);

        // Validate Category
        var categoryExists = await context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
            return Result.Failure(ProductErrors.CategoryNotFound);

        // Validate Occasion 
        if (request.OccasionId.HasValue)
        {
            var occasionExists = await context.Occasions
                .AnyAsync(o => o.Id == request.OccasionId.Value, cancellationToken);
            if (!occasionExists)
                return Result.Failure(ProductErrors.OccasionNotFound);
        }

        // Validate ProductSources
        if (request.ProductSources == null || !request.ProductSources.Any())
        {
            return Result.Failure(ProductErrors.InvalidProductSource);
        }

        if (request.ProductSources.Any(ps => string.IsNullOrWhiteSpace(ps.VendorName) || ps.Price <= 0 || string.IsNullOrWhiteSpace(ps.AffiliateLink)))
        {
            return Result.Failure(ProductErrors.InvalidProductSource);
        }
        
        var product = new Product
        {
            Id = Guid.NewGuid(),
            BrandId = request.BrandId,
            CategoryId = request.CategoryId,
            OccasionId = request.OccasionId,
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            ProductSources = request.ProductSources.Select(ps => new ProductSource
            {
                Id = Guid.NewGuid(),
                VendorName = ps.VendorName,
                Price = ps.Price,
                AffiliateLink = ps.AffiliateLink
            }).ToList()
        };
        
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(product.Id);
    }
}