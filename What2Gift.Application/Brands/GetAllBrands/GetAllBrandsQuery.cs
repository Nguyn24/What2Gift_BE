using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Brands.GetAllBrands;

public class GetAllBrandsQuery : IPageableQuery, IQuery<Page<BrandResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public class BrandResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
