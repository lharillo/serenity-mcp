using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for generating embeddings with Serenity Star
/// </summary>
[McpServerToolType]
public class EmbeddingsTools
{
    [McpServerTool, Description("Generate embeddings for text using Serenity Star's embedding models")]
    public static async Task<string> GenerateEmbeddings(
        SerenityApiClient apiClient,
        [Description("Text to generate embeddings for")] string text,
        [Description("Embedding model ID (optional, uses default if not specified)")] string? modelId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var embeddingData = new Dictionary<string, object?>
            {
                { "text", text }
            };
            
            if (!string.IsNullOrEmpty(modelId))
                embeddingData["modelId"] = modelId;

            var result = await apiClient.GenerateEmbeddingsAsync(embeddingData, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
