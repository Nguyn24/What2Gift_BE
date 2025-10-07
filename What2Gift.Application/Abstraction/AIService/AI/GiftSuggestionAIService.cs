using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using What2Gift.Application.Abstraction.AI;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Scraping;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Abstraction.AIService.AI;

public class GiftSuggestionAIService : IGiftSuggestionAiService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GiftSuggestionAIService> _logger;
    private readonly IAiEmbeddingService _embeddingService;
    private readonly IVectorDatabaseService _vectorDatabaseService;
    private readonly IDbContext _context;
    private readonly IEcommerceScrapingService _shopeeScrapingService;
    private readonly IEcommerceScrapingService _ebayScrapingService;
    private readonly IEcommerceScrapingService _googleSearchScrapingService;
    private readonly HuggingFaceChatService _huggingFaceChatService;

    public GiftSuggestionAIService(
        IConfiguration configuration,
        ILogger<GiftSuggestionAIService> logger,
        IAiEmbeddingService embeddingService,
        IVectorDatabaseService vectorDatabaseService,
        IDbContext context,
        IEcommerceScrapingService shopeeScrapingService,
        IEcommerceScrapingService ebayScrapingService,
        IEcommerceScrapingService googleSearchScrapingService,
        HuggingFaceChatService huggingFaceChatService)
    {
        _configuration = configuration;
        _logger = logger;
        _embeddingService = embeddingService;
        _vectorDatabaseService = vectorDatabaseService;
        _context = context;
        _shopeeScrapingService = shopeeScrapingService;
        _ebayScrapingService = ebayScrapingService;
        _googleSearchScrapingService = googleSearchScrapingService;
        _huggingFaceChatService = huggingFaceChatService;
    }

    public async Task<IEnumerable<Product>> GetAiSuggestedProductsAsync(GiftSuggestion giftSuggestion, CancellationToken cancellationToken = default)
    {
        try
        {
            var allProducts = new List<Product>();
            
            // 1. First, scrape from e-commerce sites to get real products
            _logger.LogInformation("Starting to scrape products from e-commerce sites for user {UserId}", giftSuggestion.UserId);
            var scrapedProducts = await GetScrapedProductsAsync(giftSuggestion, cancellationToken);
            allProducts.AddRange(scrapedProducts);
            
            _logger.LogInformation("Scraped {Count} products from e-commerce sites", scrapedProducts.Count());
            
            // 2. If not enough products from scraping, try local database
            if (allProducts.Count < 3)
            {
                _logger.LogInformation("Not enough scraped products ({Count}), trying local database", allProducts.Count);
                var localProducts = await GetLocalProductsAsync(giftSuggestion, cancellationToken);
                allProducts.AddRange(localProducts);
                _logger.LogInformation("Found {Count} additional products from local database", localProducts.Count());
            }
            
            // 3. If still not enough, use mock products as fallback
            if (allProducts.Count < 2)
            {
                _logger.LogWarning("Still not enough products ({Count}), using mock products as fallback", allProducts.Count);
                var mockProducts = await GetMockProductsAsync(giftSuggestion, cancellationToken);
                allProducts.AddRange(mockProducts);
            }
            
            // 4. Apply AI-based filtering and ranking
            var rankedProducts = await RankProductsWithAIAsync(allProducts, giftSuggestion, cancellationToken);
            
            _logger.LogInformation("Found {Count} AI suggested products for user {UserId}", 
                rankedProducts.Count(), giftSuggestion.UserId);
            
            return rankedProducts.Take(10); // Return top 10 suggestions
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting AI suggested products for user {UserId}", giftSuggestion.UserId);
            // Fallback to mock products if everything fails
            return await GetMockProductsAsync(giftSuggestion, cancellationToken);
        }
    }

    public async Task<string> GeneratePersonalizedSuggestionAsync(GiftSuggestion giftSuggestion, IEnumerable<Product> suggestedProducts, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _huggingFaceChatService.GeneratePersonalizedSuggestionAsync(giftSuggestion, suggestedProducts, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating personalized suggestion for user {UserId}", giftSuggestion.UserId);
            return "Unable to generate personalized suggestion at this time.";
        }
    }

    private async Task<IEnumerable<Product>> GetLocalProductsAsync(GiftSuggestion giftSuggestion, CancellationToken cancellationToken)
    {
        try
        {
            // Build query text from gift suggestion
            var queryText = BuildQueryText(giftSuggestion);
            _logger.LogInformation("Searching for products with query: {QueryText}", queryText);
            
            // Generate embedding for the query
            var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(queryText, cancellationToken);
            
            // Search for similar products using vector database
            var similarProductIds = await _vectorDatabaseService.SearchSimilarProductsAsync(
                queryEmbedding, 
                topK: 20, 
                threshold: 0.6f, 
                cancellationToken);
            
            _logger.LogInformation("Found {Count} similar product IDs from vector search", similarProductIds.Count());
            
            // Get products from database with budget filtering
            var products = await _context.Products
                .Include(p => p.ProductSources)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Occasion)
                .Where(p => similarProductIds.Contains(p.Id))
                .Where(p => p.ProductSources.Any(ps => 
                    ps.Price >= giftSuggestion.BudgetMin && 
                    ps.Price <= giftSuggestion.BudgetMax))
                .ToListAsync(cancellationToken);
            
            _logger.LogInformation("Found {Count} products matching budget criteria ({Min}-{Max})", 
                products.Count, giftSuggestion.BudgetMin, giftSuggestion.BudgetMax);
            
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error getting local products, will rely on scraping");
            return new List<Product>(); // Return empty list to trigger scraping
        }
    }

    private async Task<IEnumerable<Product>> GetScrapedProductsAsync(GiftSuggestion giftSuggestion, CancellationToken cancellationToken)
    {
        var products = new List<Product>();
        
        try
        {
            // Build search query from gift suggestion
            var searchQuery = BuildSearchQuery(giftSuggestion);
            _logger.LogInformation("Scraping products with search query: {SearchQuery}", searchQuery);
            
            // Scrape from Google Search first (most comprehensive)
            var googleProducts = await _googleSearchScrapingService.SearchProductsAsync(
                searchQuery, 
                giftSuggestion.BudgetMin, 
                giftSuggestion.BudgetMax, 
                cancellationToken);
            
            _logger.LogInformation("Found {Count} products from Google Search", googleProducts.Count());
            
            // Convert scraped products to domain products
            foreach (var scrapedProduct in googleProducts.Take(5))
            {
                var product = await ConvertScrapedProductToDomain(scrapedProduct, giftSuggestion);
                if (product != null)
                    products.Add(product);
            }
            
            // Scrape from Shopee if needed
            if (products.Count < 3)
            {
                _logger.LogInformation("Not enough products from Google Search ({Count}), trying Shopee", products.Count);
                
                var shopeeProducts = await _shopeeScrapingService.SearchProductsAsync(
                    searchQuery, 
                    giftSuggestion.BudgetMin, 
                    giftSuggestion.BudgetMax, 
                    cancellationToken);
                
                _logger.LogInformation("Found {Count} products from Shopee", shopeeProducts.Count());
                
                foreach (var scrapedProduct in shopeeProducts.Take(3))
                {
                    var product = await ConvertScrapedProductToDomain(scrapedProduct, giftSuggestion);
                    if (product != null)
                        products.Add(product);
                }
            }
            
            // Scrape from eBay if still needed
            if (products.Count < 2)
            {
                _logger.LogInformation("Still not enough products ({Count}), trying eBay", products.Count);
                
                var ebayProducts = await _ebayScrapingService.SearchProductsAsync(
                    searchQuery, 
                    giftSuggestion.BudgetMin, 
                    giftSuggestion.BudgetMax, 
                    cancellationToken);
                
                _logger.LogInformation("Found {Count} products from eBay", ebayProducts.Count());
                
                foreach (var scrapedProduct in ebayProducts.Take(2))
                {
                    var product = await ConvertScrapedProductToDomain(scrapedProduct, giftSuggestion);
                    if (product != null)
                        products.Add(product);
                }
            }
            
            _logger.LogInformation("Total scraped products: {Count}", products.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scraping products from e-commerce sites");
        }
        
        return products;
    }

    private async Task<IEnumerable<Product>> GetMockProductsAsync(GiftSuggestion giftSuggestion, CancellationToken cancellationToken)
    {
        // Create mock products for demo purposes
        var mockProducts = new List<Product>();
        
        // Mock music-related products
        if (giftSuggestion.RecipientHobby.ToLower().Contains("music"))
        {
            mockProducts.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Bluetooth Speaker - JBL Charge 4",
                Description = "Portable Bluetooth speaker with excellent sound quality, perfect for music lovers",
                ImageUrl = "https://example.com/jbl-speaker.jpg",
                BrandId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                OccasionId = giftSuggestion.OccasionId,
                ProductSources = new List<ProductSource>
                {
                    new ProductSource
                    {
                        Id = Guid.NewGuid(),
                        VendorName = "Amazon",
                        Price = 150000,
                        AffiliateLink = "https://amazon.com/jbl-charge-4"
                    }
                }
            });
            
            mockProducts.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Wireless Headphones - Sony WH-1000XM4",
                Description = "Premium noise-cancelling headphones with superior audio quality",
                ImageUrl = "https://example.com/sony-headphones.jpg",
                BrandId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                OccasionId = giftSuggestion.OccasionId,
                ProductSources = new List<ProductSource>
                {
                    new ProductSource
                    {
                        Id = Guid.NewGuid(),
                        VendorName = "Shopee",
                        Price = 800000,
                        AffiliateLink = "https://shopee.vn/sony-wh1000xm4"
                    }
                }
            });
            
            mockProducts.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Music Box - Wooden Hand-crafted",
                Description = "Beautiful wooden music box perfect for music enthusiasts",
                ImageUrl = "https://example.com/music-box.jpg",
                BrandId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                OccasionId = giftSuggestion.OccasionId,
                ProductSources = new List<ProductSource>
                {
                    new ProductSource
                    {
                        Id = Guid.NewGuid(),
                        VendorName = "Lazada",
                        Price = 500000,
                        AffiliateLink = "https://lazada.vn/music-box-wooden"
                    }
                }
            });
        }
        
        // Mock general gift products with various price ranges
        mockProducts.Add(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Smart Watch - Apple Watch Series 9",
            Description = "Latest Apple Watch with health monitoring and fitness tracking",
            ImageUrl = "https://example.com/apple-watch.jpg",
            BrandId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            OccasionId = giftSuggestion.OccasionId,
            ProductSources = new List<ProductSource>
            {
                new ProductSource
                {
                    Id = Guid.NewGuid(),
                    VendorName = "Apple Store",
                    Price = 12000000,
                    AffiliateLink = "https://apple.com/watch-series-9"
                }
            }
        });
        
        // Add more products with different price ranges
        mockProducts.Add(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Gaming Mouse - Logitech G Pro X",
            Description = "Professional gaming mouse with high precision sensor",
            ImageUrl = "https://example.com/gaming-mouse.jpg",
            BrandId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            OccasionId = giftSuggestion.OccasionId,
            ProductSources = new List<ProductSource>
            {
                new ProductSource
                {
                    Id = Guid.NewGuid(),
                    VendorName = "Shopee",
                    Price = 2500000,
                    AffiliateLink = "https://shopee.vn/logitech-g-pro-x"
                }
            }
        });
        
        mockProducts.Add(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mechanical Keyboard - Keychron K8",
            Description = "Wireless mechanical keyboard with RGB lighting",
            ImageUrl = "https://example.com/mechanical-keyboard.jpg",
            BrandId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            OccasionId = giftSuggestion.OccasionId,
            ProductSources = new List<ProductSource>
            {
                new ProductSource
                {
                    Id = Guid.NewGuid(),
                    VendorName = "Lazada",
                    Price = 1800000,
                    AffiliateLink = "https://lazada.vn/keychron-k8"
                }
            }
        });
        
        mockProducts.Add(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Wireless Earbuds - AirPods Pro 2",
            Description = "Premium wireless earbuds with active noise cancellation",
            ImageUrl = "https://example.com/airpods-pro.jpg",
            BrandId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            OccasionId = giftSuggestion.OccasionId,
            ProductSources = new List<ProductSource>
            {
                new ProductSource
                {
                    Id = Guid.NewGuid(),
                    VendorName = "Apple Store",
                    Price = 6500000,
                    AffiliateLink = "https://apple.com/airpods-pro-2"
                }
            }
        });
        
        // Filter by budget
        var filteredProducts = mockProducts.Where(p => 
            p.ProductSources.Any(ps => 
                ps.Price >= giftSuggestion.BudgetMin && 
                ps.Price <= giftSuggestion.BudgetMax)).ToList();
        
        _logger.LogInformation("Generated {Count} mock products, {FilteredCount} within budget", 
            mockProducts.Count, filteredProducts.Count);
        
        return filteredProducts;
    }

    private Task<IEnumerable<Product>> RankProductsWithAIAsync(IEnumerable<Product> products, GiftSuggestion giftSuggestion, CancellationToken cancellationToken)
    {
        // Simple ranking based on relevance to user preferences
        // In a more sophisticated implementation, you could use AI to rank products
        return Task.FromResult(products.OrderByDescending(p => CalculateRelevanceScore(p, giftSuggestion)).AsEnumerable());
    }

    private static int CalculateRelevanceScore(Product product, GiftSuggestion giftSuggestion)
    {
        var score = 0;
        
        // Age appropriateness
        if (IsAgeAppropriate(product, giftSuggestion.RecipientAge))
            score += 10;
        
        // Hobby relevance
        if (IsHobbyRelevant(product, giftSuggestion.RecipientHobby))
            score += 20;
        
        // Occasion match
        if (giftSuggestion.Occasion != null && product.OccasionId == giftSuggestion.Occasion.Id)
            score += 15;
        
        // Price within budget
        var avgPrice = product.ProductSources.Average(ps => ps.Price);
        if (avgPrice >= giftSuggestion.BudgetMin && avgPrice <= giftSuggestion.BudgetMax)
            score += 10;
        
        return score;
    }

    private async Task<Product?> ConvertScrapedProductToDomain(ScrapedProduct scrapedProduct, GiftSuggestion giftSuggestion)
    {
        try
        {
            // Find or create brand
            var brandId = await FindOrCreateBrand(scrapedProduct.Brand);
            
            // Find or create category
            var categoryId = await FindOrCreateCategory(scrapedProduct.Category);
            
            // Create a product from scraped data
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = scrapedProduct.Name,
                Description = scrapedProduct.Description,
                ImageUrl = scrapedProduct.ImageUrl,
                BrandId = brandId,
                CategoryId = categoryId,
                OccasionId = giftSuggestion.OccasionId,
                ProductSources = new List<ProductSource>
                {
                    new ProductSource
                    {
                        Id = Guid.NewGuid(),
                        VendorName = scrapedProduct.VendorName,
                        Price = scrapedProduct.Price,
                        AffiliateLink = scrapedProduct.ProductUrl
                    }
                }
            };
            
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error converting scraped product to domain: {ProductName}", scrapedProduct.Name);
            return null;
        }
    }

    private static string BuildSearchQuery(GiftSuggestion giftSuggestion)
    {
        var queryParts = new List<string>();
        
        // Add hobby as main search term
        queryParts.Add(giftSuggestion.RecipientHobby);
        
        // Add occasion if available
        if (giftSuggestion.Occasion != null)
        {
            queryParts.Add(giftSuggestion.Occasion.Name);
        }
        
        // Add gender-specific terms
        if (giftSuggestion.RecipientGender == RecipientGender.Male)
        {
            queryParts.Add("men");
        }
        else
        {
            queryParts.Add("women");
        }
        
        return string.Join(" ", queryParts);
    }

    private static string BuildQueryText(GiftSuggestion giftSuggestion)
    {
        var queryBuilder = new StringBuilder();
        
        queryBuilder.AppendLine($"Gift for {giftSuggestion.RecipientGender} aged {giftSuggestion.RecipientAge}");
        queryBuilder.AppendLine($"Hobbies: {giftSuggestion.RecipientHobby}");
        queryBuilder.AppendLine($"Budget: ${giftSuggestion.BudgetMin} - ${giftSuggestion.BudgetMax}");
        
        if (giftSuggestion.Occasion != null)
        {
            queryBuilder.AppendLine($"Occasion: {giftSuggestion.Occasion.Name}");
        }
        
        return queryBuilder.ToString();
    }

    private static IEnumerable<Product> FilterProductsByRecipientCharacteristics(IEnumerable<Product> products, GiftSuggestion giftSuggestion)
    {
        // Apply age-appropriate filtering
        var ageFilteredProducts = products.Where(p => IsAgeAppropriate(p, giftSuggestion.RecipientAge));
        
        // Apply hobby-based filtering
        var hobbyFilteredProducts = ageFilteredProducts.Where(p => 
            IsHobbyRelevant(p, giftSuggestion.RecipientHobby));
        
        return hobbyFilteredProducts;
    }

    private static bool IsAgeAppropriate(Product product, int recipientAge)
    {
        // Simple age appropriateness logic - can be enhanced with more sophisticated rules
        var productName = product.Name.ToLower();
        var description = product.Description?.ToLower() ?? "";
        
        // Filter out inappropriate products based on age
        if (recipientAge < 13)
        {
            return !productName.Contains("adult") && !description.Contains("adult");
        }
        
        return true;
    }

    private static bool IsHobbyRelevant(Product product, string hobby)
    {
        var productText = $"{product.Name} {product.Description}".ToLower();
        var hobbyLower = hobby.ToLower();
        
        // Simple keyword matching - can be enhanced with more sophisticated NLP
        return productText.Contains(hobbyLower) || 
               hobbyLower.Split(' ').Any(h => productText.Contains(h));
    }

    private static string BuildPersonalizationPrompt(GiftSuggestion giftSuggestion, IEnumerable<Product> suggestedProducts)
    {
        var promptBuilder = new StringBuilder();
        
        promptBuilder.AppendLine("Based on the following gift preferences, provide a personalized gift suggestion:");
        promptBuilder.AppendLine($"- Recipient: {giftSuggestion.RecipientGender}, {giftSuggestion.RecipientAge} years old");
        promptBuilder.AppendLine($"- Hobbies: {giftSuggestion.RecipientHobby}");
        promptBuilder.AppendLine($"- Budget: ${giftSuggestion.BudgetMin} - ${giftSuggestion.BudgetMax}");
        
        if (giftSuggestion.Occasion != null)
        {
            promptBuilder.AppendLine($"- Occasion: {giftSuggestion.Occasion.Name}");
        }
        
        promptBuilder.AppendLine("\nAvailable products:");
        foreach (var product in suggestedProducts.Take(5))
        {
            promptBuilder.AppendLine($"- {product.Name}: {product.Description} (${product.ProductSources.FirstOrDefault()?.Price ?? 0})");
        }
        
        promptBuilder.AppendLine("\nPlease provide a thoughtful, personalized gift recommendation explaining why this gift would be perfect for the recipient.");
        
        return promptBuilder.ToString();
    }

    private record OpenAIChatResponse(
        List<Choice> Choices,
        string Model,
        Usage Usage);

    private record Choice(
        Message Message,
        string FinishReason,
        int Index);

    private record Message(
        string Role,
        string Content);

    private record Usage(
        int PromptTokens,
        int CompletionTokens,
        int TotalTokens);

    private async Task<Guid> FindOrCreateBrand(string brandName)
    {
        try
        {
            if (string.IsNullOrEmpty(brandName) || brandName == "Unknown")
            {
                // Return a default brand ID or create a generic brand
                var defaultBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Generic");
                if (defaultBrand != null)
                    return defaultBrand.Id;
                
                // Create generic brand if not exists
                var newBrand = new Brand { Id = Guid.NewGuid(), Name = "Generic" };
                _context.Brands.Add(newBrand);
                await _context.SaveChangesAsync();
                return newBrand.Id;
            }
            
            // Try to find existing brand
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name.ToLower() == brandName.ToLower());
            if (existingBrand != null)
                return existingBrand.Id;
            
            // Create new brand
            var brand = new Brand { Id = Guid.NewGuid(), Name = brandName };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand.Id;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error finding or creating brand: {BrandName}", brandName);
            // Return a default brand ID
            var defaultBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Generic");
            return defaultBrand?.Id ?? Guid.Empty;
        }
    }

    private async Task<Guid> FindOrCreateCategory(string categoryName)
    {
        try
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                // Return a default category ID
                var defaultCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "General");
                if (defaultCategory != null)
                    return defaultCategory.Id;
                
                // Create general category if not exists
                var newCategory = new Category { Id = Guid.NewGuid(), Name = "General" };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
                return newCategory.Id;
            }
            
            // Try to find existing category
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
            if (existingCategory != null)
                return existingCategory.Id;
            
            // Create new category
            var category = new Category { Id = Guid.NewGuid(), Name = categoryName };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error finding or creating category: {CategoryName}", categoryName);
            // Return a default category ID
            var defaultCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "General");
            return defaultCategory?.Id ?? Guid.Empty;
        }
    }
}
