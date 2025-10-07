using What2Gift.Domain.Products;

namespace What2Gift.Application.Abstraction.AI;

public interface IAiEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, float[]>> GenerateProductEmbeddingsAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default);
}
