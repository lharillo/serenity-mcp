using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing Serenity Star agent versions
/// </summary>
[McpServerToolType]
public class AgentVersionTools
{
    // ================================================================================
    // VERSION LISTING & RETRIEVAL
    // ================================================================================

    [McpServerTool, Description("List all versions of a specific agent with pagination and filtering")]
    public static async Task<string> ListAgentVersions(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("Page number (default: 1)")] int page = 1,
        [Description("Page size (default: 20)")] int pageSize = 20,
        [Description("Filter from version number (optional)")] int? fromVersion = null,
        [Description("Filter to version number (optional)")] int? toVersion = null,
        [Description("Filter from date (ISO 8601 format, optional)")] string? fromDate = null,
        [Description("Filter to date (ISO 8601 format, optional)")] string? toDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };

            if (fromVersion.HasValue) queryParams.Add($"fromVersion={fromVersion.Value}");
            if (toVersion.HasValue) queryParams.Add($"toVersion={toVersion.Value}");
            if (!string.IsNullOrEmpty(fromDate)) queryParams.Add($"fromDate={fromDate}");
            if (!string.IsNullOrEmpty(toDate)) queryParams.Add($"toDate={toDate}");

            var query = string.Join("&", queryParams);
            var result = await apiClient.GetAgentVersionsAsync(agentCode, query, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get the current (latest saved) version of an agent with full configuration")]
    public static async Task<string> GetCurrentAgentVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetAgentDetailsAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get the published (active) version of an agent")]
    public static async Task<string> GetPublishedAgentVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetPublishedAgentVersionAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get a specific version of an agent by version number")]
    public static async Task<string> GetAgentVersionByNumber(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The version number")] int versionNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetAgentVersionByNumberAsync(agentCode, versionNumber, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // DRAFT MANAGEMENT
    // ================================================================================

    [McpServerTool, Description("Create a new draft version from the current version of an agent")]
    public static async Task<string> CreateAgentDraft(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.CreateAgentDraftAsync(agentCode, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Create a new draft version from a specific version of an agent")]
    public static async Task<string> CreateAgentDraftFromVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The version number to create draft from")] int versionNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.CreateAgentDraftFromVersionAsync(agentCode, versionNumber, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Convert a draft version into a normal saved version without publishing it")]
    public static async Task<string> SaveDraftVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The draft version number to save")] int versionNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.SaveDraftVersionAsync(agentCode, versionNumber, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // PUBLISH MANAGEMENT
    // ================================================================================

    [McpServerTool, Description("Publish a specific version, making it the current active version")]
    public static async Task<string> PublishAgentVersion(
        SerenityApiClient apiClient,
        [Description("The agent code/identifier")] string agentCode,
        [Description("The version number to publish")] int versionNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.PublishAgentVersionAsync(agentCode, versionNumber, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
