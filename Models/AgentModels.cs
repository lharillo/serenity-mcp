namespace SerenityStarMcp.Models;

public record AgentExecuteRequest(
    string AgentCode,
    string Message,
    string Channel = "Jarvis",
    string UserIdentifier = "jarvis.starkcloud@gmail.com",
    string? ChatId = null,
    string[]? VolatileKnowledgeIds = null
);

public record AgentResponse(
    string Content,
    CompletionUsage CompletionUsage,
    Cost Cost,
    string InstanceId,
    string UserMessageId,
    string AgentMessageId
);

public record CompletionUsage(
    int CompletionTokens,
    int PromptTokens,
    int TotalTokens
);

public record Cost(
    decimal Completion,
    decimal Prompt,
    decimal Total,
    string Currency
);

public record ConversationResponse(string ChatId);

public record FeedbackRequest(
    int Rating,
    string? Comment = null
);