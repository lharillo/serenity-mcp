using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for account management (login, logout, token refresh)
/// </summary>
[McpServerToolType]
public class AccountTools
{
    [McpServerTool, Description("Get current user information")]
    public static async Task<string> GetCurrentUser(
        SerenityApiClient apiClient,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetCurrentUserAsync(cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Login user and obtain authentication token")]
    public static async Task<string> LoginUser(
        SerenityApiClient apiClient,
        [Description("User email")] string email,
        [Description("User password")] string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.LoginAsync(email, password, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Logout current user")]
    public static async Task<string> LogoutUser(
        SerenityApiClient apiClient,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.LogoutAsync(cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Refresh authentication token")]
    public static async Task<string> RefreshToken(
        SerenityApiClient apiClient,
        [Description("Refresh token")] string refreshToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.RefreshTokenAsync(refreshToken, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
