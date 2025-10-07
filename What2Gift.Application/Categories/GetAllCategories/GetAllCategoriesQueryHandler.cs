using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Categories.GetAllCategories;

public class GetAllCategoriesQueryHandler(IDbContext context) : IQueryHandler<GetAllCategoriesQuery, Page<CategoryResponse>>
{
    public async Task<Result<Page<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Categories.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync(cancellationToken);

        return new Page<CategoryResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
