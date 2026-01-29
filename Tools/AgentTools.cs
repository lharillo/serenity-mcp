using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

[McpServerToolType]
public class AgentTools
{
    [McpServerTool, Description("Execute a Serenity Star AI agent with a message")]
    public static async Task<string> ExecuteAgent(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier to execute")] string agentCode,
        [Description("The message to send to the agent")] string message,
        [Description("Optional channel identifier (default: MCP)")] string channel = "MCP",
        [Description("User identifier for tracking (default: mcp-user)")] string userIdentifier = "mcp-user",
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.ExecuteAgentAsync(agentCode, message, channel, userIdentifier, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (HttpRequestException ex)
        {
            return $"Error executing agent '{agentCode}': {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }

    [McpServerTool, Description("List all available Serenity Star agents")]
    public static async Task<string> ListAgents(
        SerenityApiClient apiClient,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var agents = await apiClient.GetAgentsAsync(cancellationToken);
            return JsonSerializer.Serialize(agents, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (HttpRequestException ex)
        {
            return $"Error fetching agents: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get detailed information about a specific agent")]
    public static async Task<string> GetAgentDetails(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var agentDetails = await apiClient.GetAgentDetailsAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(agentDetails, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (HttpRequestException ex)
        {
            return $"Error fetching agent '{agentCode}': {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }
}