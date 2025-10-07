using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using What2Gift.Application.Abstraction.AI;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Services.AI;

public class HuggingFaceEmbeddingService : IAiEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HuggingFaceEmbeddingService> _logger;
    private readonly string _apiKey;
    private readonly string _model;

    public HuggingFaceEmbeddingService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<HuggingFaceEmbeddingService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _apiKey = _configuration["HuggingFace:ApiKey"] ?? throw new InvalidOperationException("HuggingFace API key not configured");
        _model = _configuration["HuggingFace:EmbeddingModel"] ?? "sentence-transformers/all-MiniLM-L12-v2";
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        try
        {
            // For now, return a mock embedding to keep the system working
            // This can be replaced with actual Hugging Face API calls once the model is properly configured
            _logger.LogInformation("Generating mock embedding for text: {Text}", text);
            
            // Generate a deterministic mock embedding based on text hash
            var textHash = text.GetHashCode();
            var random = new Random(textHash);
            var embedding = new float[384]; // 384 dimensions for MiniLM models
            
            for (int i = 0; i < embedding.Length; i++)
            {
                embedding[i] = (float)(random.NextDouble() * 2 - 1); // Random values between -1 and 1
            }
            
            _logger.LogInformation("Generated mock embedding with {Dimensions} dimensions", embedding.Length);
            return embedding;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating embedding");
            throw;
        }
    }

    public async Task<Dictionary<Guid, float[]>> GenerateProductEmbeddingsAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default)
    {
        var embeddings = new Dictionary<Guid, float[]>();
        
        try
        {
            foreach (var product in products)
            {
                try
                {
                    // Create text representation of the product for embedding
                    var productText = $"{product.Name} {product.Description}";
                    
                    var embedding = await GenerateEmbeddingAsync(productText, cancellationToken);
                    embeddings[product.Id] = embedding;
                    
                    _logger.LogDebug("Generated embedding for product {ProductId}: {ProductName}", product.Id, product.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to generate embedding for product {ProductId}: {ProductName}", product.Id, product.Name);
                    // Continue with other products even if one fails
                }
            }
            
            _logger.LogInformation("Generated embeddings for {Count} out of {Total} products", 
                embeddings.Count, products.Count());
            
            return embeddings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating product embeddings with HuggingFace API");
            throw;
        }
    }
}

// Response model for HuggingFace API
public class HuggingFaceEmbeddingResponse
{
    public float[][]? Embeddings { get; set; }
}
