using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Products.SearchProductByBudget;

public class GetProductByBudgetQuery : IPageableQuery, IQuery<Page<GetProductResponse>>
{
    public decimal MinPrice { get; init; }
    public decimal MaxPrice { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}