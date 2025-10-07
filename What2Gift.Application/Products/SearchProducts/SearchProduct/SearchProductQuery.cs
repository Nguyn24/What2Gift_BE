using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Products.SearchProducts.SearchProduct;

public class SearchProductQuery : IPageableQuery, IQuery<Page<GetProductResponse>>
{
    public Guid? BrandId { get; init; }
    public Guid? CategoryId { get; init; }
    public Guid? OccasionId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
