using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for accessing agent insights and analytics
/// </summary>
[McpServerToolType]
public class InsightsTools
{
    [McpServerTool, Description("Get insights analytics for a specific agent")]
    public static async Task<string> GetInsightsByAgent(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetInsightsByAgentAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get insights analytics for a specific agent version")]
    public static async Task<string> GetInsightsByVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The agent version number")] int agentVersion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetInsightsByVersionAsync(agentCode, agentVersion, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get insights analytics for a specific agent instance")]
    public static async Task<string> GetInsightsByInstance(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The agent instance ID")] string agentInstanceId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetInsightsByInstanceAsync(agentCode, agentInstanceId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
