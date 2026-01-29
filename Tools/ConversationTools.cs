using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing conversation context and variables
/// </summary>
[McpServerToolType]
public class ConversationTools
{
    [McpServerTool, Description("Get context variable list for an agent")]
    public static async Task<string> GetContextList(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetContextListAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get context variables for a specific agent version")]
    public static async Task<string> GetContextByVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The agent version number")] int agentVersion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetContextByVersionAsync(agentCode, agentVersion, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get conversation context for a specific conversation")]
    public static async Task<string> GetConversationContext(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The conversation ID")] string conversationId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetConversationContextAsync(agentCode, conversationId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update context variables for a conversation")]
    public static async Task<string> UpdateContextVariables(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The conversation ID")] string conversationId,
        [Description("Context variables as JSON object")] string contextVariablesJson,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var contextVariables = JsonSerializer.Deserialize<Dictionary<string, object>>(contextVariablesJson);
            if (contextVariables == null)
            {
                return JsonSerializer.Serialize(new { error = "Invalid context variables JSON" });
            }

            var result = await apiClient.UpdateContextVariablesAsync(agentCode, conversationId, contextVariables, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
