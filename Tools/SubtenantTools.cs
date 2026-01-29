using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing subtenants
/// </summary>
[McpServerToolType]
public class SubtenantTools
{
    [McpServerTool, Description("List all subtenants with pagination")]
    public static async Task<string> ListSubtenants(
        SerenityApiClient apiClient,
        [Description("Page number (default: 1)")] int page = 1,
        [Description("Page size (default: 20)")] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.ListSubtenantsAsync(page, pageSize, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
