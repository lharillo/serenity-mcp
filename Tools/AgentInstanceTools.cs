using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing agent instances
/// </summary>
[McpServerToolType]
public class AgentInstanceTools
{
    [McpServerTool, Description("Get all agent instances from Serenity Star platform")]
    public static async Task<string> ListAgentInstances(
        SerenityApiClient apiClient,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetAgentInstancesAsync(cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
