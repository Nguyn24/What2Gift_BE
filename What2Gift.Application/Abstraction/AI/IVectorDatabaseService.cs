using What2Gift.Domain.Products;

namespace What2Gift.Application.Abstraction.AI;

public interface IVectorDatabaseService
{
    Task CreateCollectionAsync(string collectionName, CancellationToken cancellationToken = default);
    Task UpsertProductVectorsAsync(IEnumerable<Product> products, Dictionary<Guid, float[]> embeddings, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guid>> SearchSimilarProductsAsync(float[] queryEmbedding, int topK = 10, float threshold = 0.7f, CancellationToken cancellationToken = default);
}
