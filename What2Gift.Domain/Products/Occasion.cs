using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products;

public class Occasion : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateOnly DateRangeStart { get; set; }
    public DateOnly DateRangeEnd { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<GiftSuggestion> GiftSuggestions { get; set; } = new List<GiftSuggestion>();
}