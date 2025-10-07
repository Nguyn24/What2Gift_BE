using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Products.AISuggestion;

public record GetAiSuggestedProductsCommand(
    Guid? OccasionId,
    RecipientGender RecipientGender,
    int RecipientAge,
    string RecipientHobby,
    decimal BudgetMin,
    decimal BudgetMax
) : ICommand<GetProductResponse>;
