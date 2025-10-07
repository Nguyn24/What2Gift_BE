using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Application.Products.AISuggestion;
using What2Gift.Domain.Products;

namespace What2Gift.Apis.Controller;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class AiSuggestionController(ISender sender) : ControllerBase
{
    [HttpPost("suggest")]
    public async Task<IResult> GetAiSuggestedProducts(
        [FromBody] GetAiSuggestedProductsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GetAiSuggestedProductsCommand(
            OccasionId: request.OccasionId,
            RecipientGender: request.RecipientGender,
            RecipientAge: request.RecipientAge,
            RecipientHobby: request.RecipientHobby,
            BudgetMin: request.BudgetMin,
            BudgetMax: request.BudgetMax
        );

        var result = await sender.Send(command, cancellationToken);

        return result.MatchOk();
    }
    
    public record GetAiSuggestedProductsRequest(
        Guid? OccasionId,
        RecipientGender RecipientGender,
        int RecipientAge,
        string RecipientHobby,
        decimal BudgetMin,
        decimal BudgetMax
    );
    
}