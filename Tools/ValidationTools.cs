using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for dataset and table schema validation
/// </summary>
[McpServerToolType]
public class ValidationTools
{
    [McpServerTool, Description("Validate dataset file schema")]
    public static async Task<string> ValidateDatasetSchema(
        SerenityApiClient apiClient,
        [Description("Base64 encoded file content")] string fileContentBase64,
        [Description("File name with extension")] string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = Convert.FromBase64String(fileContentBase64);
            var result = await apiClient.ValidateDatasetSchemaAsync(fileContent, fileName, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Validate table file schema for a specific dataset table")]
    public static async Task<string> ValidateTableSchema(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        [Description("Base64 encoded file content")] string fileContentBase64,
        [Description("File name with extension")] string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = Convert.FromBase64String(fileContentBase64);
            var result = await apiClient.ValidateTableSchemaAsync(datasetId, tableId, fileContent, fileName, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
