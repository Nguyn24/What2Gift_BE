using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Products.GetAllProduct;

public class GetProductQuery : IPageableQuery, IQuery<Page<GetProductResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}