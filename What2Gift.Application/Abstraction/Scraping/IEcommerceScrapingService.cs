using What2Gift.Domain.Products;

namespace What2Gift.Application.Abstraction.Scraping;

public interface IEcommerceScrapingService
{
    Task<IEnumerable<ScrapedProduct>> SearchProductsAsync(string query, decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<ScrapedProduct>> GetProductsByCategoryAsync(string category, decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
}

public record ScrapedProduct(
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    string ProductUrl,
    string VendorName,
    string Category,
    string Brand,
    float Rating,
    int ReviewCount
);
