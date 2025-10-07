using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using What2Gift.Domain.Products;

namespace What2Gift.Infrastructure.Services.AI;

public class HuggingFaceChatService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HuggingFaceChatService> _logger;
    private readonly string _apiKey;
    private readonly string _model;

    public HuggingFaceChatService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<HuggingFaceChatService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _apiKey = _configuration["HuggingFace:ApiKey"] ??
                  throw new InvalidOperationException("HuggingFace API key not configured");
        _model = _configuration["HuggingFace:ChatModel"] ?? "microsoft/DialoGPT-medium";

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<string> GeneratePersonalizedSuggestionAsync(GiftSuggestion giftSuggestion,
        IEnumerable<Product> suggestedProducts, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = BuildPersonalizationPrompt(giftSuggestion, suggestedProducts);

            var requestBody = new
            {
                inputs = prompt,
                parameters = new
                {
                    max_new_tokens = 500,
                    temperature = 0.7,
                    return_full_text = false
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://api-inference.huggingface.co/models/{_model}", content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("HuggingFace Chat API error: {StatusCode} - {Error}", response.StatusCode,
                    errorContent);
                throw new HttpRequestException($"HuggingFace Chat API error: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var chatResponse = JsonSerializer.Deserialize<HuggingFaceChatResponse[]>(responseContent);

            if (chatResponse == null || chatResponse.Length == 0)
            {
                _logger.LogError("Invalid response from HuggingFace Chat API");
                return "I apologize, but I'm having trouble generating a personalized suggestion at the moment.";
            }

            return chatResponse[0].GeneratedText?.Trim() ??
                   "I apologize, but I'm having trouble generating a personalized suggestion at the moment.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating personalized suggestion with HuggingFace API");
            return "I apologize, but I'm having trouble generating a personalized suggestion at the moment.";
        }
    }

    private string BuildPersonalizationPrompt(GiftSuggestion giftSuggestion, IEnumerable<Product> suggestedProducts)
    {
        var productsList = string.Join(", ", suggestedProducts.Select(p => p.Name));

        return $@"Based on the following gift suggestion request, provide a personalized recommendation:

Recipient: {giftSuggestion.RecipientGender} aged {giftSuggestion.RecipientAge}
Occasion: {giftSuggestion.Occasion?.Name ?? "General"}
Budget: ${giftSuggestion.BudgetMin} - ${giftSuggestion.BudgetMax}
Hobbies/Interests: {string.Join(", ", giftSuggestion.RecipientHobby)}

Available products: {productsList}

Please provide a personalized gift recommendation explaining why these products would be perfect for this recipient. Keep it friendly and helpful, around 2-3 sentences.";
    }
}

// Response model for HuggingFace Chat API
public class HuggingFaceChatResponse
{
    public string? GeneratedText { get; set; }
}