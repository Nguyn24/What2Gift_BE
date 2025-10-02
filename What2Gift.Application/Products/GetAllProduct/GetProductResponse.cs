namespace What2Gift.Application.Products.GetAllProduct;

public class GetProductResponse
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<ProductSources> ProductSources { get; set; } = new();
}

public class ProductSources
{
    public Guid Id { get; set; }
    public string VendorName { get; set; } = null!;
    public decimal Price { get; set; }
    public string AffiliateLink { get; set; } = null!;
}