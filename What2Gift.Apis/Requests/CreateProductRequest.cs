
namespace What2Gift.Apis.Requests;

public class CreateProductRequest
{
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }

    public List<ProductSourcesRequest> ProductSources { get; set; } = new();
}

public class ProductSourcesRequest
{
    public string VendorName { get; set; } = null!;
    public decimal Price { get; set; }
    public string AffiliateLink { get; set; } = null!;
}
