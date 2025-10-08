using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Brands.CreateBrand;

public class CreateBrandCommandHandler(IDbContext context) : ICommandHandler<CreateBrandCommand>
{
    public async Task<Result> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new Brand
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        context.Brands.Add(brand);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(brand.Id);
    }
}
