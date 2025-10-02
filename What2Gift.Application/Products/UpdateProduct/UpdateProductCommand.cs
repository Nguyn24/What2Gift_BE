using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Products.CreateProduct;

namespace What2Gift.Application.Products.UpdateProduct;

public class UpdateProductCommand : ICommand
{
    public Guid Id { get; set; } 
    public Guid? BrandId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public string? Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<UpdateProductSourceRequest> ProductSources { get; set; } = new();
}
public class UpdateProductSourceRequest
{
    public Guid? Id { get; set; }                   
    public string VendorName { get; set; } = null!;
    public decimal Price { get; set; }
    public string AffiliateLink { get; set; } = null!;
}