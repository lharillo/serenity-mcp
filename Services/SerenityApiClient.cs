using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace SerenityStarMcp.Services;

public class SerenityApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;

    public SerenityApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = GetApiKey();
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    private string GetApiKey()
    {
        // Try environment variable first
        var apiKey = Environment.GetEnvironmentVariable("SERENITY_API_KEY");
        if (!string.IsNullOrEmpty(apiKey))
            return apiKey;

        // Fall back to configuration
        apiKey = _configuration["SerenityApi:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("Serenity API key not found. Set SERENITY_API_KEY environment variable or configure SerenityApi:ApiKey in appsettings.json");

        return apiKey;
    }

    public async Task<JsonElement> GetAgentsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/Agent", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<JsonElement>(content);
    }

    public async Task<JsonElement> GetAgentDetailsAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/Agent/{agentCode}", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<JsonElement>(content);
    }

    public async Task<JsonElement> ExecuteAgentAsync(string agentCode, string message, string channel = "MCP", string userIdentifier = "mcp-user", CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            parameters = new[]
            {
                new { Key = "message", Value = message },
                new { Key = "channel", Value = channel },
                new { Key = "userIdentifier", Value = userIdentifier }
            }
        };

        var jsonContent = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"api/v2/agent/{agentCode}/execute", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<JsonElement>(content);
    }

    public async Task<JsonElement> GetModelsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/v2/aimodel?pageSize=500", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<JsonElement>(content);
    }
}