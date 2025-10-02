using What2Gift.Application.Products.UpdateProduct;

namespace What2Gift.Apis.Requests;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public string? Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; } 
    public List<UpdateProductSourceRequest> ProductSources { get; set; } = new();
}