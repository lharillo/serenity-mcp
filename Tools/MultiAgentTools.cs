using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing multiple agent types (Activity, Copilot, Chat, AIProxy)
/// </summary>
[McpServerToolType]
public class MultiAgentTools
{
    // ================================================================================
    // ACTIVITY AGENT
    // ================================================================================

    [McpServerTool, Description("Create a new Activity agent (workflow automation agent)")]
    public static async Task<string> CreateActivityAgent(
        SerenityApiClient apiClient,
        [Description("Agent name")] string name,
        [Description("Unique agent code (lowercase, hyphenated)")] string code,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.CreateAgentAsync("activity", name, code, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update an existing Activity agent")]
    public static async Task<string> UpdateActivityAgent(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAgentAsync("activity", agentCode, name, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update and publish an Activity agent with version state control")]
    public static async Task<string> UpdateActivityAgentWithVersion(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Version state (draft/publish)")] string versionState,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAgentWithVersionAsync("activity", agentCode, versionState, name, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // COPILOT AGENT
    // ================================================================================

    [McpServerTool, Description("Create a new Copilot agent (interactive assistant with real-time suggestions)")]
    public static async Task<string> CreateCopilotAgent(
        SerenityApiClient apiClient,
        [Description("Agent name")] string name,
        [Description("Unique agent code (lowercase, hyphenated)")] string code,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.CreateAgentAsync("copilot", name, code, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update an existing Copilot agent")]
    public static async Task<string> UpdateCopilotAgent(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAgentAsync("copilot", agentCode, name, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update and publish a Copilot agent with version state control")]
    public static async Task<string> UpdateCopilotAgentWithVersion(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Version state (draft/publish)")] string versionState,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAgentWithVersionAsync("copilot", agentCode, versionState, name, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // CHAT AGENT
    // ================================================================================

    [McpServerTool, Description("Create a new Chat agent (chat completion agent for conversational AI)")]
    public static async Task<string> CreateChatAgent(
        SerenityApiClient apiClient,
        [Description("Agent name")] string name,
        [Description("Unique agent code (lowercase, hyphenated)")] string code,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.CreateAgentAsync("chat", name, code, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update an existing Chat agent")]
    public static async Task<string> UpdateChatAgent(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAgentAsync("chat", agentCode, name, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update and publish a Chat agent with version state control")]
    public static async Task<string> UpdateChatAgentWithVersion(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Version state (draft/publish)")] string versionState,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("System definition/prompt")] string systemDefinition,
        [Description("Initial welcome message")] string initialMessage,
        [Description("Model UUID")] string modelId,
        [Description("Conversation starters as JSON array (optional)")] string? conversationStarters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAgentWithVersionAsync("chat", agentCode, versionState, name, description, systemDefinition, initialMessage, modelId, conversationStarters, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // AI PROXY AGENT
    // ================================================================================

    [McpServerTool, Description("Create a new AI Proxy agent (direct model access without processing)")]
    public static async Task<string> CreateAIProxyAgent(
        SerenityApiClient apiClient,
        [Description("Agent name")] string name,
        [Description("Unique agent code (lowercase, hyphenated)")] string code,
        [Description("Agent description")] string description,
        [Description("Model UUID")] string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // AI Proxy agents are simpler - no system definition or messages
            var result = await apiClient.CreateAIProxyAgentAsync(name, code, description, modelId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update an existing AI Proxy agent")]
    public static async Task<string> UpdateAIProxyAgent(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("Model UUID")] string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAIProxyAgentAsync(agentCode, name, description, modelId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update and publish an AI Proxy agent with version state control")]
    public static async Task<string> UpdateAIProxyAgentWithVersion(
        SerenityApiClient apiClient,
        [Description("Agent code")] string agentCode,
        [Description("Version state (draft/publish)")] string versionState,
        [Description("Agent name")] string name,
        [Description("Agent description")] string description,
        [Description("Model UUID")] string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.UpdateAIProxyAgentWithVersionAsync(agentCode, versionState, name, description, modelId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
