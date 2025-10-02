using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products;

public class Occasion : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int StartMonth { get; set; }
    public int StartDay { get; set; }
    public int EndMonth { get; set; }
    public int EndDay { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<GiftSuggestion> GiftSuggestions { get; set; } = new List<GiftSuggestion>();
}