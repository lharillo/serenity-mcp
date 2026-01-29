using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for permanent knowledge file management
/// </summary>
[McpServerToolType]
public class KnowledgeTools
{
    [McpServerTool, Description("Upload a knowledge file (permanent, for agent knowledge base)")]
    public static async Task<string> UploadKnowledgeFile(
        SerenityApiClient apiClient,
        [Description("Base64 encoded file content")] string fileContentBase64,
        [Description("File name with extension")] string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = Convert.FromBase64String(fileContentBase64);
            var result = await apiClient.UploadKnowledgeFileAsync(fileContent, fileName, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Upload a knowledge file for a specific agent")]
    public static async Task<string> UploadKnowledgeFileForAgent(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Base64 encoded file content")] string fileContentBase64,
        [Description("File name with extension")] string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = Convert.FromBase64String(fileContentBase64);
            var result = await apiClient.UploadKnowledgeFileForAgentAsync(agentCode, fileContent, fileName, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Delete a knowledge file by ID")]
    public static async Task<string> DeleteKnowledgeFile(
        SerenityApiClient apiClient,
        [Description("Knowledge file ID")] string fileId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.DeleteKnowledgeFileAsync(fileId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
