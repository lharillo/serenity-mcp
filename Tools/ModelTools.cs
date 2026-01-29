using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

[McpServerToolType]
public class ModelTools
{
    [McpServerTool, Description("List all available AI models from Serenity Star platform")]
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
            return $"Error fetching models: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }
}