using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Brands.DeleteBrand;

public class DeleteBrandCommandHandler(IDbContext context) : ICommandHandler<DeleteBrandCommand>
{
    public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (brand is null)
        {
            return Result.Failure(Error.NotFound("Brand.NotFound", "Brand not found"));
        }

        // Check if brand has products
        var hasProducts = await context.Products
            .AnyAsync(p => p.BrandId == request.Id, cancellationToken);

        if (hasProducts)
        {
            return Result.Failure(Error.Problem("Brand.HasProducts", "Cannot delete brand that has products"));
        }

        context.Brands.Remove(brand);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
