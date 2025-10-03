using MediatR;
using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products.Errors;

namespace What2Gift.Application.Products.DeleteProduct;

public class DeleteProductCommandHandler(IDbContext context)
: ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .Include(p => p.ProductSources)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
       
        if (product == null)
            return Result.Failure<Guid>(ProductErrors.ProductNotFound);
        
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(product.Id);
    }
}