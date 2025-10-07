using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Categories.CreateCategory;

public class CreateCategoryCommandHandler(IDbContext context) : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);
    }
}
