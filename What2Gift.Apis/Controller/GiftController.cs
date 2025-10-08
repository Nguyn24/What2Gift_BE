using Microsoft.AspNetCore.Mvc;
using What2Gift.Infrastructure.Services;

namespace What2Gift.Apis.Controller;

[ApiController]
[Route("api/")]
public class GiftController : ControllerBase
{
    private readonly AiSuggestionService _aiService;

    public GiftController(AiSuggestionService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("suggest")]
    public async Task<IActionResult> SuggestGift([FromBody] GiftPrompt prompt)
    {
        var result = await _aiService.GetGiftSuggestions(prompt);
        return Content(result, "application/json");
    }
    
    public class GiftPrompt
    {
        public string? gift_recipient { get; set; }
        public string? sex { get; set; }
        public string? occasion { get; set; }
        public string? Preferences { get; set; }
        public string? budget { get; set; }
    }
}