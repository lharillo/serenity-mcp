using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing channel configurations
/// </summary>
[McpServerToolType]
public class ChannelTools
{
    [McpServerTool, Description("Get channel configuration for a specific agent")]
    public static async Task<string> GetChannelConfig(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetChannelConfigAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
