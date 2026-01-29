using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for file management with Serenity Star
/// </summary>
[McpServerToolType]
public class FileTools
{
    [McpServerTool, Description("Upload a file to Serenity Star")]
    public static async Task<string> UploadFile(
        SerenityApiClient apiClient,
        [Description("Base64 encoded file content")] string fileContentBase64,
        [Description("File name with extension")] string fileName,
        [Description("File MIME type (e.g., 'application/pdf', 'image/png')")] string mimeType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = Convert.FromBase64String(fileContentBase64);
            var result = await apiClient.UploadFileAsync(fileContent, fileName, mimeType, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get file information by ID")]
    public static async Task<string> GetFileInfo(
        SerenityApiClient apiClient,
        [Description("File ID")] string fileId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetFileInfoAsync(fileId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Download a file by ID (returns base64 encoded content)")]
    public static async Task<string> DownloadFile(
        SerenityApiClient apiClient,
        [Description("File ID")] string fileId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.DownloadFileAsync(fileId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
