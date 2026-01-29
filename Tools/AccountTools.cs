using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for account information
/// </summary>
[McpServerToolType]
public class AccountTools
{
    [McpServerTool, Description("Get information about the currently authenticated user account")]
    public static async Task<string> GetCurrentAccount(
        SerenityApiClient apiClient,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var account = await apiClient.GetCurrentAccountAsync(cancellationToken);
            return JsonSerializer.Serialize(account, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
