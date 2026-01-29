using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for discovering available AI models
/// </summary>
[McpServerToolType]
public class ModelTools
{
    [McpServerTool, Description("List all available AI models from Serenity Star platform with UUIDs")]
    public static async Task<string> ListModels(
        SerenityApiClient apiClient,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var models = await apiClient.GetModelsAsync(cancellationToken);
            return JsonSerializer.Serialize(models, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (HttpRequestException ex)
        {
            return JsonSerializer.Serialize(new { error = $"HTTP error: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
