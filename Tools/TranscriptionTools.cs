using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for audio/video transcription with Serenity Star
/// </summary>
[McpServerToolType]
public class TranscriptionTools
{
    [McpServerTool, Description("Transcribe an audio or video file")]
    public static async Task<string> TranscribeAudioFile(
        SerenityApiClient apiClient,
        [Description("Base64 encoded audio/video file content")] string fileContentBase64,
        [Description("File name with extension")] string fileName,
        [Description("Language code (e.g., 'en', 'es', optional)")] string? language = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = Convert.FromBase64String(fileContentBase64);
            var result = await apiClient.TranscribeAudioAsync(fileContent, fileName, language, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Transcribe an audio or video file by file ID (file must be already uploaded)")]
    public static async Task<string> TranscribeAudioByFileId(
        SerenityApiClient apiClient,
        [Description("File ID (uploaded file)")] string fileId,
        [Description("Language code (e.g., 'en', 'es', optional)")] string? language = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.TranscribeAudioByFileIdAsync(fileId, language, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
