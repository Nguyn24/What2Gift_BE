using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products;

public class Product : Entity
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Brand Brand { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Occasion? Occasion { get; set; }
    public ICollection<ProductSource> ProductSources { get; set; } = new List<ProductSource>();
    public ICollection<GiftSuggestion> GiftSuggestions { get; set; } = new List<GiftSuggestion>();
}