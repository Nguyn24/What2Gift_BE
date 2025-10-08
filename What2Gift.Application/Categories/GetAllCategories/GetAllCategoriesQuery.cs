using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Categories.GetAllCategories;

public class GetAllCategoriesQuery : IPageableQuery, IQuery<Page<CategoryResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public class CategoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
