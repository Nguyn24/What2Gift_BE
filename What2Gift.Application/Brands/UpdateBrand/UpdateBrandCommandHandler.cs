using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Brands.UpdateBrand;

public class UpdateBrandCommandHandler(IDbContext context) : ICommandHandler<UpdateBrandCommand>
{
    public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (brand is null)
        {
            return Result.Failure(Error.NotFound("Brand.NotFound", "Brand not found"));
        }

        brand.Name = request.Name;
        brand.Description = request.Description;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
