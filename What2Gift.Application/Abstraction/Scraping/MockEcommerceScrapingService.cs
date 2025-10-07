using Microsoft.Extensions.Logging;
using What2Gift.Application.Abstraction.Scraping;

namespace What2Gift.Application.Abstraction.Scraping;

public class MockEcommerceScrapingService : IEcommerceScrapingService
{
    private readonly ILogger<MockEcommerceScrapingService> _logger;
    private readonly string _serviceName;

    public MockEcommerceScrapingService(ILogger<MockEcommerceScrapingService> logger, string serviceName = "Mock")
    {
        _logger = logger;
        _serviceName = serviceName;
    }

    public async Task<IEnumerable<ScrapedProduct>> SearchProductsAsync(string query, decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Mock {ServiceName} scraping service searching for: {Query} (${MinPrice}-${MaxPrice})", 
            _serviceName, query, minPrice, maxPrice);

        // Return empty list for now - this is just a mock implementation
        // In a real implementation, this would scrape actual e-commerce sites
        await Task.Delay(100, cancellationToken); // Simulate some processing time
        
        return new List<ScrapedProduct>();
    }

    public async Task<IEnumerable<ScrapedProduct>> GetProductsByCategoryAsync(string category, decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Mock {ServiceName} scraping service searching category: {Category} (${MinPrice}-${MaxPrice})", 
            _serviceName, category, minPrice, maxPrice);

        // Return empty list for now - this is just a mock implementation
        await Task.Delay(100, cancellationToken); // Simulate some processing time
        
        return new List<ScrapedProduct>();
    }
}
