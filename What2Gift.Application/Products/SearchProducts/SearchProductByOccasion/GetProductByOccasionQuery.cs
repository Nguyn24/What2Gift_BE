using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Products.SearchProducts.SearchProductByOccasion;

public class GetProductByOccasionQuery : IPageableQuery, IQuery<Page<GetProductResponse>>
{
    public Guid OccasionId { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}