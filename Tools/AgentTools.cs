using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using SerenityStarMcp.Models;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing Serenity Star AI agents
/// </summary>
[McpServerToolType]
public class AgentTools
{
    // ================================================================================
    // READ OPERATIONS
    // ================================================================================

    [McpServerTool, Description("List all available Serenity Star agents")]
    public static async Task<string> ListAgents(
        SerenityApiClient apiClient,
        [Description("Number of agents to retrieve per page (default: 50)")] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var agents = await apiClient.GetAgentsAsync(pageSize, cancellationToken);
            return JsonSerializer.Serialize(agents, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (HttpRequestException ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
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
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // CREATE OPERATIONS
    // ================================================================================

    [McpServerTool, Description("Create a new Serenity Star Assistant agent")]
    public static async Task<string> CreateAssistantAgent(
        SerenityApiClient apiClient,
        [Description("Agent name")] string name,
        [Description("Unique agent code (lowercase, hyphenated)")] string code,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID (e.g., 76ef01a0-392d-2088-7b91-3a13d971c604 for gpt-4o-mini)")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        [Description("Enable web search skill (optional)")] bool enableWebSearch = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var starters = new List<string>();
            if (!string.IsNullOrEmpty(conversationStarters))
            {
                try
                {
                    starters = JsonSerializer.Deserialize<List<string>>(conversationStarters) ?? new List<string>();
                }
                catch
                {
                    // If JSON parsing fails, treat as single starter
                    starters = new List<string> { conversationStarters };
                }
            }

            // API requires PascalCase for Create endpoint
            var agentData = new
            {
                Name = name,
                Code = code,
                Description = description,
                General = new
                {
                    Code = code,
                    Name = name,
                    Starters = starters
                },
                Behaviour = new
                {
                    SystemDefinition = systemDefinition,
                    InitialMessage = initialMessage
                },
                Model = new
                {
                    Main = new
                    {
                        Id = modelId
                    }
                },
                Skills = new
                {
                    WebSearch = new { Enabled = enableWebSearch }
                }
            };

            var result = await apiClient.CreateAssistantAgentAsync(agentData, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ================================================================================
    // UPDATE OPERATIONS
    // ================================================================================

    [McpServerTool, Description("Update an existing Serenity Star Assistant agent (without publishing)")]
    public static async Task<string> UpdateAssistantAgent(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("Updated agent data as JSON (camelCase format)")] string agentDataJson,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var agentData = JsonSerializer.Deserialize<object>(agentDataJson);
            var result = await apiClient.UpdateAssistantAgentAsync(agentCode, agentData!, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update and publish an existing Serenity Star Assistant agent")]
    public static async Task<string> UpdateAndPublishAssistantAgent(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("Updated agent data as JSON (camelCase format)")] string agentDataJson,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var agentData = JsonSerializer.Deserialize<object>(agentDataJson);
            var result = await apiClient.UpdateAndPublishAssistantAgentAsync(agentCode, agentData!, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // EXECUTE OPERATIONS
    // ================================================================================

    [McpServerTool, Description("Execute a Serenity Star AI agent with a message")]
    public static async Task<string> ExecuteAgent(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier to execute")] string agentCode,
        [Description("The message to send to the agent")] string message,
        [Description("Channel identifier (default: MCP)")] string channel = "MCP",
        [Description("User identifier for tracking (default: mcp-client)")] string userIdentifier = "mcp-client",
        [Description("Optional conversation ID for stateful conversations")] string? chatId = null,
        [Description("Optional volatile knowledge file IDs (comma-separated)")] string? volatileKnowledgeIds = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var knowledgeIds = string.IsNullOrEmpty(volatileKnowledgeIds) 
                ? null 
                : volatileKnowledgeIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                
            var result = await apiClient.ExecuteAgentAsync(agentCode, message, channel, userIdentifier, chatId, knowledgeIds, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (HttpRequestException ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // CONVERSATION OPERATIONS
    // ================================================================================

    [McpServerTool, Description("Create a conversation for stateful agent interactions")]
    public static async Task<string> CreateConversation(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.CreateConversationAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Create conversation info for an agent with optional context variables")]
    public static async Task<string> CreateConversationInfo(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("Optional context variables as JSON string")] string? contextVariables = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Dictionary<string, object>? context = null;
            if (!string.IsNullOrEmpty(contextVariables))
            {
                context = JsonSerializer.Deserialize<Dictionary<string, object>>(contextVariables);
            }

            var result = await apiClient.CreateConversationInfoAsync(agentCode, context, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get conversation info for a specific agent version with optional context variables")]
    public static async Task<string> GetConversationInfoByVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The agent version number")] int agentVersion,
        [Description("Optional context variables as JSON string")] string? contextVariables = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Dictionary<string, object>? context = null;
            if (!string.IsNullOrEmpty(contextVariables))
            {
                context = JsonSerializer.Deserialize<Dictionary<string, object>>(contextVariables);
            }

            var result = await apiClient.GetConversationInfoByVersionAsync(agentCode, agentVersion, context, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get conversation details by conversation ID")]
    public static async Task<string> GetConversation(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The conversation ID")] string conversationId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetConversationAsync(agentCode, conversationId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // TOKEN USAGE
    // ================================================================================

    [McpServerTool, Description("Get token usage statistics for an agent")]
    public static async Task<string> GetTokenUsage(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("Optional start date (YYYY-MM-DD format)")] string? startDate = null,
        [Description("Optional end date (YYYY-MM-DD format)")] string? endDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            DateTime? start = string.IsNullOrEmpty(startDate) ? null : DateTime.Parse(startDate);
            DateTime? end = string.IsNullOrEmpty(endDate) ? null : DateTime.Parse(endDate);

            var request = new TokenUsageRequest(agentCode, start, end);
            var result = await apiClient.GetTokenUsageAsync(request, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // FEEDBACK OPERATIONS
    // ================================================================================

    [McpServerTool, Description("Submit feedback for an agent message")]
    public static async Task<string> SubmitFeedback(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The conversation ID")] string conversationId,
        [Description("The agent message ID")] string agentMessageId,
        [Description("Rating (1-5)")] int rating,
        [Description("Optional comment")] string? comment = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var feedback = new FeedbackRequest(rating, comment);
            var result = await apiClient.SubmitFeedbackAsync(agentCode, conversationId, agentMessageId, feedback, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Delete feedback for an agent message")]
    public static async Task<string> DeleteFeedback(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The conversation ID")] string conversationId,
        [Description("The agent message ID")] string agentMessageId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.DeleteFeedbackAsync(agentCode, conversationId, agentMessageId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
