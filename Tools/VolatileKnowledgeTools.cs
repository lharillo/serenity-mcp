using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing volatile knowledge (temporary document uploads for agent context)
/// </summary>
[McpServerToolType]
public class VolatileKnowledgeTools
{
    [McpServerTool, Description("Upload a document for temporary use in agent execution (base64 encoded)")]
    public static async Task<string> UploadVolatileKnowledge(
        SerenityApiClient apiClient,
        [Description("Base64 encoded file content")] string base64Content,
        [Description("File name with extension (e.g., document.pdf)")] string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileBytes = Convert.FromBase64String(base64Content);
            var result = await apiClient.UploadVolatileKnowledgeAsync(fileBytes, fileName, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (FormatException)
        {
            return JsonSerializer.Serialize(new { error = "Invalid base64 content" });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
