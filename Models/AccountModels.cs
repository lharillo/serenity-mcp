namespace SerenityStarMcp.Models;

public record LoginRequest(string Email, string Password);

public record RefreshTokenRequest(string RefreshToken);

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    AccountInfo User
);

public record AccountInfo(
    string Id,
    string Email,
    string Name,
    string Role
);

public record TokenUsageRequest(string AgentCode, DateTime? StartDate = null, DateTime? EndDate = null);

public record ConversationInfoRequest(
    string AgentCode,
    Dictionary<string, object>? ContextVariables = null
);

public record UpdateContextRequest(Dictionary<string, object> ContextVariables);