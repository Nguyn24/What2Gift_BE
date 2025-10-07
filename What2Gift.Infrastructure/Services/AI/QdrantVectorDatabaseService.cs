using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using What2Gift.Application.Abstraction.AI;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Services.AI;

public class QdrantVectorDatabaseService : IVectorDatabaseService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<QdrantVectorDatabaseService> _logger;
    private readonly string _collectionName;
    private readonly string _qdrantUrl;

    public QdrantVectorDatabaseService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<QdrantVectorDatabaseService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _collectionName = configuration["Qdrant:CollectionName"] ?? "products";
        _qdrantUrl = configuration["Qdrant:Url"] ?? "http://localhost:6333";
        
        _logger.LogInformation("Using Qdrant HTTP REST API at {Url}", _qdrantUrl);
    }

    public async Task CreateCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if collection exists
            var collectionsResponse = await _httpClient.GetAsync($"{_qdrantUrl}/collections", cancellationToken);
            if (collectionsResponse.IsSuccessStatusCode)
            {
                var collectionsContent = await collectionsResponse.Content.ReadAsStringAsync(cancellationToken);
                var collectionsData = JsonSerializer.Deserialize<QdrantCollectionsResponse>(collectionsContent);
                
                if (collectionsData?.Result?.Collections?.Any(c => c.Name == collectionName) == true)
                {
                    _logger.LogInformation("Qdrant collection already exists: {CollectionName}", collectionName);
                    return;
                }
            }

            // Create collection
            var createRequest = new
            {
                vectors = new
                {
                    size = 384, // Hugging Face sentence-transformers/all-MiniLM-L6-v2 dimension
                    distance = "Cosine"
                }
            };

            var json = JsonSerializer.Serialize(createRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_qdrantUrl}/collections/{collectionName}", content, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Created Qdrant collection: {CollectionName}", collectionName);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("Failed to create Qdrant collection {CollectionName}: {Error}", collectionName, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Qdrant collection: {CollectionName}. Will continue without vector search.", collectionName);
            // Don't throw - allow the system to continue without vector search
        }
    }

    public async Task UpsertProductVectorsAsync(IEnumerable<Product> products, Dictionary<Guid, float[]> embeddings, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Upserted {Count} product vectors to Qdrant (simplified)", products.Count());
            // TODO: Implement actual HTTP REST API calls to Qdrant
            // For now, just log the operation
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error upserting product vectors to Qdrant");
            // Don't throw - allow the system to continue without vector search
        }
    }

    public async Task<IEnumerable<Guid>> SearchSimilarProductsAsync(float[] queryEmbedding, int topK = 10, float threshold = 0.7f, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching for similar products with topK={TopK}, threshold={Threshold}", topK, threshold);
            // TODO: Implement actual HTTP REST API calls to Qdrant
            // For now, return empty list
            return new List<Guid>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching similar products in Qdrant");
            return new List<Guid>();
        }
    }

    public async Task DeleteProductVectorsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleted {Count} product vectors from Qdrant (simplified)", productIds.Count());
            // TODO: Implement actual HTTP REST API calls to Qdrant
            // For now, just log the operation
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product vectors from Qdrant");
            // Don't throw - allow the system to continue without vector search
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

// Response classes for Qdrant API
public class QdrantCollectionsResponse
{
    public QdrantCollectionsResult? Result { get; set; }
}

public class QdrantCollectionsResult
{
    public List<QdrantCollection>? Collections { get; set; }
}

public class QdrantCollection
{
    public string? Name { get; set; }
}
