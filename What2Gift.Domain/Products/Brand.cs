using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products;

public class Brand : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}