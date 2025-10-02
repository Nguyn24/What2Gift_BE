using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Products.CreateProduct;

public class CreateProductCommand : ICommand
{
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<CreateProductSourcesRequest> ProductSources { get; set; } = new();
}

public class CreateProductSourcesRequest
{
    public string VendorName { get; set; } = null!;
    public decimal Price { get; set; }
    public string AffiliateLink { get; set; } = null!;
}