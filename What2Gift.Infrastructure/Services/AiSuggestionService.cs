using System.Text;
using System.Text.Json;
namespace What2Gift.Infrastructure.Services;

public class AiSuggestionService
{
    private readonly HttpClient _httpClient;
    private const string AiApiBaseUrl = "https://what2gift-py.onrender.com"; 
    public AiSuggestionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetGiftSuggestions(object prompt)
    {
        var json = JsonSerializer.Serialize(prompt);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{AiApiBaseUrl}/products", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}