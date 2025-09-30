using What2Gift.Domain.Affiliate;
using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products;

public class ProductSource : Entity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string VendorName { get; set; } = null!;
    public decimal Price { get; set; }
    public string AffiliateLink { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ICollection<AffiliateClick> AffiliateClicks { get; set; } = new List<AffiliateClick>();
}