using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(IDbContext context) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure(Error.NotFound("Category.NotFound", "Category not found"));
        }

        // Check if category has products
        var hasProducts = await context.Products
            .AnyAsync(p => p.CategoryId == request.Id, cancellationToken);

        if (hasProducts)
        {
            return Result.Failure(Error.Problem("Category.HasProducts", "Cannot delete category that has products"));
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
