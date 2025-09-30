using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products;

public class Category : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}