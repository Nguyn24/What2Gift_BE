using What2Gift.Domain.Products;

namespace What2Gift.Application.Abstraction.AI;

public interface IGiftSuggestionAiService
{
    Task<IEnumerable<Product>> GetAiSuggestedProductsAsync(GiftSuggestion giftSuggestion, CancellationToken cancellationToken = default);
}
