using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using SerenityStarMcp.Models;

namespace SerenityStarMcp.Services;

/// <summary>
/// HTTP client for interacting with the Serenity Star AI Platform API
/// </summary>
/// <remarks>
/// Provides methods for managing AI agents, models, conversations, prompts, and other Serenity Star resources.
/// API key is provided by MCP clients via X-Serenity-API-Key header for security.
/// </remarks>
public class SerenityApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the Serenity API client
    /// </summary>
    /// <param name="httpClient">Configured HTTP client instance</param>
    /// <param name="configuration">Application configuration</param>
    /// <param name="httpContextAccessor">HTTP context accessor to read request headers</param>
    public SerenityApiClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the API key from the current HTTP request header
    /// </summary>
    private string GetApiKey()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new InvalidOperationException("No HTTP context available");

        if (context.Request.Headers.TryGetValue("X-Serenity-API-Key", out var apiKey))
        {
            return apiKey.ToString();
        }

        throw new UnauthorizedAccessException("X-Serenity-API-Key header is required");
    }

    /// <summary>
    /// Creates an HTTP request message with the API key from request headers
    /// </summary>
    private async Task<HttpResponseMessage> SendWithApiKeyAsync(HttpMethod method, string uri, HttpContent? content = null, CancellationToken cancellationToken = default)
    {
        var apiKey = GetApiKey();
        
        using var request = new HttpRequestMessage(method, uri);
        request.Headers.Add("X-API-KEY", apiKey);
        
        if (content != null)
            request.Content = content;
            
        return await _httpClient.SendAsync(request, cancellationToken);
    }

    // ================================================================================
    // AGENT ENDPOINTS
    // ================================================================================

    /// <summary>
    /// List all available agents
    /// </summary>
    public async Task<JsonElement> GetAgentsAsync(int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/Agent?pageSize={pageSize}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Get detailed information about a specific agent (current version)
    /// </summary>
    /// <remarks>
    /// Returns the current version of the agent with full configuration including:
    /// - systemDefinition
    /// - initialMessage
    /// - conversationStarters
    /// - model configuration
    /// - knowledge sources
    /// </remarks>
    public async Task<JsonElement> GetAgentDetailsAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/AgentVersion/{agentCode}/Current", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Create a new Assistant agent
    /// </summary>
    /// <remarks>
    /// Uses PascalCase for field names as required by the API
    /// </remarks>
    public async Task<JsonElement> CreateAssistantAgentAsync(object agentData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/Agent/assistant", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Update an existing Assistant agent without publishing
    /// </summary>
    /// <remarks>
    /// Uses camelCase for field names (different from Create endpoint)
    /// </remarks>
    public async Task<JsonElement> UpdateAssistantAgentAsync(string agentCode, object agentData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/agent/assistant/{agentCode}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Update and publish an existing Assistant agent
    /// </summary>
    /// <remarks>
    /// Uses camelCase for field names (different from Create endpoint)
    /// </remarks>
    public async Task<JsonElement> UpdateAndPublishAssistantAgentAsync(string agentCode, object agentData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/agent/assistant/{agentCode}/publish", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // MULTI-AGENT TYPE ENDPOINTS (Activity, Copilot, Chat, AIProxy)
    // ================================================================================

    /// <summary>
    /// Create a new agent of specific type (activity, copilot, chat)
    /// Activity and Chat use "Instructions", Copilot uses "Behaviour"
    /// </summary>
    public async Task<JsonElement> CreateAgentAsync(string agentType, string name, string code, string description, string systemDefinition, string initialMessage, string modelId, string? conversationStarters, CancellationToken cancellationToken = default)
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
                starters = new List<string> { conversationStarters };
            }
        }

        object agentData;
        
        // Activity and Chat use "Instructions", Copilot uses "Behaviour"
        if (agentType == "activity" || agentType == "chat")
        {
            agentData = new
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
                Instructions = new
                {
                    SystemDefinition = systemDefinition
                },
                Behaviour = new
                {
                    InitialMessage = initialMessage
                },
                Model = new
                {
                    Main = new
                    {
                        Id = modelId
                    }
                }
            };
        }
        else // copilot
        {
            agentData = new
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
                }
            };
        }

        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/agent/{agentType}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Update an existing agent of specific type
    /// </summary>
    public async Task<JsonElement> UpdateAgentAsync(string agentType, string agentCode, string name, string description, string systemDefinition, string initialMessage, string modelId, string? conversationStarters, CancellationToken cancellationToken = default)
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
                starters = new List<string> { conversationStarters };
            }
        }

        var agentData = new
        {
            general = new
            {
                name = name,
                description = description
            },
            behaviour = new
            {
                systemDefinition = systemDefinition,
                initialMessage = initialMessage,
                conversationStarters = starters
            },
            model = new
            {
                main = new
                {
                    id = modelId
                }
            },
            knowledge = new
            {
                knowledgeSources = new List<object>(),
                datasetSources = new List<object>()
            }
        };

        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/agent/{agentType}/{agentCode}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Update and control version state of an agent
    /// </summary>
    public async Task<JsonElement> UpdateAgentWithVersionAsync(string agentType, string agentCode, string versionState, string name, string description, string systemDefinition, string initialMessage, string modelId, string? conversationStarters, CancellationToken cancellationToken = default)
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
                starters = new List<string> { conversationStarters };
            }
        }

        var agentData = new
        {
            general = new
            {
                name = name,
                description = description
            },
            behaviour = new
            {
                systemDefinition = systemDefinition,
                initialMessage = initialMessage,
                conversationStarters = starters
            },
            model = new
            {
                main = new
                {
                    id = modelId
                }
            },
            knowledge = new
            {
                knowledgeSources = new List<object>(),
                datasetSources = new List<object>()
            }
        };

        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/agent/{agentType}/{agentCode}/{versionState}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Create AI Proxy agent (simpler schema)
    /// </summary>
    public async Task<JsonElement> CreateAIProxyAgentAsync(string name, string code, string description, string modelId, CancellationToken cancellationToken = default)
    {
        var agentData = new
        {
            Name = name,
            Code = code,
            Description = description,
            Model = new
            {
                Main = new
                {
                    Id = modelId
                }
            }
        };

        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/agent/aiproxy", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Update AI Proxy agent
    /// </summary>
    public async Task<JsonElement> UpdateAIProxyAgentAsync(string agentCode, string name, string description, string modelId, CancellationToken cancellationToken = default)
    {
        var agentData = new
        {
            general = new
            {
                name = name,
                description = description
            },
            model = new
            {
                main = new
                {
                    id = modelId
                }
            }
        };

        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/agent/aiproxy/{agentCode}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Update AI Proxy agent with version state
    /// </summary>
    public async Task<JsonElement> UpdateAIProxyAgentWithVersionAsync(string agentCode, string versionState, string name, string description, string modelId, CancellationToken cancellationToken = default)
    {
        var agentData = new
        {
            general = new
            {
                name = name,
                description = description
            },
            model = new
            {
                main = new
                {
                    id = modelId
                }
            }
        };

        var jsonContent = JsonSerializer.Serialize(agentData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/agent/aiproxy/{agentCode}/{versionState}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // AGENT VERSION ENDPOINTS
    // ================================================================================

    /// <summary>
    /// List all versions of a specific agent
    /// </summary>
    public async Task<JsonElement> GetAgentVersionsAsync(string agentCode, string queryString, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/AgentVersion/{agentCode}?{queryString}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Get the published (active) version of an agent
    /// </summary>
    public async Task<JsonElement> GetPublishedAgentVersionAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/AgentVersion/{agentCode}/Published", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Get a specific version of an agent by version number
    /// </summary>
    public async Task<JsonElement> GetAgentVersionByNumberAsync(string agentCode, int versionNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/AgentVersion/{agentCode}/{versionNumber}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Create a new draft version from the current version
    /// </summary>
    public async Task<JsonElement> CreateAgentDraftAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/AgentVersion/{agentCode}/Draft", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Create a new draft version from a specific version
    /// </summary>
    public async Task<JsonElement> CreateAgentDraftFromVersionAsync(string agentCode, int versionNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/AgentVersion/{agentCode}/Draft/{versionNumber}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Convert a draft version into a normal saved version without publishing
    /// </summary>
    public async Task<JsonElement> SaveDraftVersionAsync(string agentCode, int versionNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/AgentVersion/{agentCode}/Draft/{versionNumber}/Save", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Publish a specific version, making it the current active version
    /// </summary>
    public async Task<JsonElement> PublishAgentVersionAsync(string agentCode, int versionNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Put, $"api/v2/AgentVersion/{agentCode}/Publish/{versionNumber}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // AGENT EXECUTION & CONVERSATION ENDPOINTS
    // ================================================================================

    /// <summary>
    /// Execute an agent with a message
    /// </summary>
    public async Task<JsonElement> ExecuteAgentAsync(string agentCode, string message, string channel = "MCP", string userIdentifier = "mcp-client", string? chatId = null, string[]? volatileKnowledgeIds = null, CancellationToken cancellationToken = default)
    {
        var parameters = new List<object>
        {
            new { Key = "message", Value = message },
            new { Key = "channel", Value = channel },
            new { Key = "userIdentifier", Value = userIdentifier }
        };

        if (!string.IsNullOrEmpty(chatId))
            parameters.Add(new { Key = "chatId", Value = chatId });

        if (volatileKnowledgeIds != null && volatileKnowledgeIds.Length > 0)
            parameters.Add(new { Key = "volatileKnowledgeIds", Value = volatileKnowledgeIds });

        var jsonContent = JsonSerializer.Serialize(parameters);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/agent/{agentCode}/execute", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Create a conversation for stateful agent interactions
    /// </summary>
    public async Task<JsonElement> CreateConversationAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/agent/{agentCode}/conversation", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> CreateConversationInfoAsync(string agentCode, Dictionary<string, object>? contextVariables = null, CancellationToken cancellationToken = default)
    {
        var payload = new { contextVariables = contextVariables ?? new Dictionary<string, object>() };
        var jsonContent = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/Agent/{agentCode}/conversation/info", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetConversationInfoByVersionAsync(string agentCode, int agentVersion, Dictionary<string, object>? contextVariables = null, CancellationToken cancellationToken = default)
    {
        var payload = new { contextVariables = contextVariables ?? new Dictionary<string, object>() };
        var jsonContent = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/Agent/{agentCode}/{agentVersion}/conversation/info", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetTokenUsageAsync(TokenUsageRequest request, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/agent/token-usage", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetConversationAsync(string agentCode, string conversationId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/Agent/{agentCode}/conversation/{conversationId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> SubmitFeedbackAsync(string agentCode, string conversationId, string agentMessageId, FeedbackRequest feedback, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(feedback);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/Agent/{agentCode}/conversation/{conversationId}/message/{agentMessageId}/feedback", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> DeleteFeedbackAsync(string agentCode, string conversationId, string agentMessageId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Delete, $"api/v2/Agent/{agentCode}/conversation/{conversationId}/message/{agentMessageId}/feedback", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // CONVERSATION & CONTEXT ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GetContextListAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/agent/{agentCode}/conversation/context", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetContextByVersionAsync(string agentCode, int agentVersion, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/agent/{agentCode}/{agentVersion}/conversation/context", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetConversationContextAsync(string agentCode, string conversationId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/agent/{agentCode}/conversation/{conversationId}/context", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> UpdateContextVariablesAsync(string agentCode, string conversationId, Dictionary<string, object> contextVariables, CancellationToken cancellationToken = default)
    {
        var payload = new { contextVariables };
        var jsonContent = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Patch, $"api/v2/agent/{agentCode}/conversation/{conversationId}/context", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // AGENT INSTANCE ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GetAgentInstancesAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, "api/v2/AgentInstance", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // MODEL ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GetModelsAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, "api/v2/aimodel?pageSize=500", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // KNOWLEDGE MANAGEMENT ENDPOINTS (Permanent)
    // ================================================================================

    /// <summary>
    /// Upload a knowledge file (permanent, for agent knowledge base)
    /// </summary>
    public async Task<JsonElement> UploadKnowledgeFileAsync(byte[] fileContent, string fileName, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileContent);
        fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileStreamContent, "File", fileName);

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/KnowledgeFile/upload", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Upload a knowledge file for a specific agent
    /// </summary>
    public async Task<JsonElement> UploadKnowledgeFileForAgentAsync(string agentCode, byte[] fileContent, string fileName, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileContent);
        fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileStreamContent, "File", fileName);

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/KnowledgeFile/upload/{agentCode}", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    /// <summary>
    /// Delete a knowledge file
    /// </summary>
    public async Task<JsonElement> DeleteKnowledgeFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var payload = new { fileId };
        var jsonContent = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Delete, "api/v2/KnowledgeFile", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // VOLATILE KNOWLEDGE ENDPOINTS (Document Upload)
    // ================================================================================

    /// <summary>
    /// Upload a document for volatile knowledge (temporary context for agent execution)
    /// </summary>
    public async Task<JsonElement> UploadVolatileKnowledgeAsync(byte[] fileContent, string fileName, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileContent);
        fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileStreamContent, "File", fileName);

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/VolatileKnowledge", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // DATASET ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> ListDatasetsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/Dataset?page={page}&pageSize={pageSize}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> CreateDatasetAsync(object datasetData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(datasetData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/Dataset", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetDatasetAsync(string datasetId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/Dataset/{datasetId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> UpdateDatasetAsync(string datasetId, object updateData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(updateData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(new HttpMethod("PATCH"), $"api/v2/Dataset/{datasetId}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> DeleteDatasetAsync(string datasetId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Delete, $"api/v2/Dataset/{datasetId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> QueryDatasetAsync(string datasetId, object queryData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(queryData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/Dataset/{datasetId}/query", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> CreateTableAsync(string datasetId, object tableData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(tableData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, $"api/v2/Dataset/{datasetId}/table", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> UpdateTableAsync(string datasetId, string tableId, object updateData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(updateData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(new HttpMethod("PATCH"), $"api/v2/Dataset/{datasetId}/table/{tableId}", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> DeleteTableAsync(string datasetId, string tableId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Delete, $"api/v2/Dataset/{datasetId}/table/{tableId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> AppendToTableAsync(string datasetId, string tableId, object appendData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(appendData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(new HttpMethod("PATCH"), $"api/v2/Dataset/{datasetId}/table/{tableId}/AppendTable", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> ReplaceTableDataAsync(string datasetId, string tableId, object replaceData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(replaceData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(new HttpMethod("PATCH"), $"api/v2/Dataset/{datasetId}/table/{tableId}/ReplaceTable", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // EMBEDDINGS ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GenerateEmbeddingsAsync(object embeddingData, CancellationToken cancellationToken = default)
    {
        var jsonContent = JsonSerializer.Serialize(embeddingData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/Embeddings/generate", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // TRANSCRIPTION ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> TranscribeAudioAsync(byte[] fileContent, string fileName, string? language, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileContent);
        fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileStreamContent, "File", fileName);
        
        if (!string.IsNullOrEmpty(language))
            content.Add(new StringContent(language), "language");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/Audio/transcribe", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> TranscribeAudioByFileIdAsync(string fileId, string? language, CancellationToken cancellationToken = default)
    {
        var payload = new Dictionary<string, object?> { { "fileId", fileId } };
        if (!string.IsNullOrEmpty(language))
            payload["language"] = language;

        var jsonContent = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/Audio/transcribe/file", httpContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // FILE MANAGEMENT ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> UploadFileAsync(byte[] fileContent, string fileName, string mimeType, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileContent);
        fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
        content.Add(fileStreamContent, "File", fileName);

        var response = await SendWithApiKeyAsync(HttpMethod.Post, "api/v2/File/upload", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetFileInfoAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/File/{fileId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/File/download/{fileId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        var base64 = Convert.ToBase64String(content);
        return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(new { fileId, contentBase64 = base64 }));
    }

    // ================================================================================
    // CHANNEL CONFIGURATION ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GetChannelConfigAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/channel/serenity-chat/{agentCode}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // INSIGHTS ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GetInsightsByAgentAsync(string agentCode, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/agent/{agentCode}/insights", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetInsightsByVersionAsync(string agentCode, int agentVersion, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/agent/{agentCode}/{agentVersion}/insights", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    public async Task<JsonElement> GetInsightsByInstanceAsync(string agentCode, string agentInstanceId, CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, $"api/v2/agent/{agentCode}/insights/{agentInstanceId}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // ACCOUNT ENDPOINTS
    // ================================================================================

    public async Task<JsonElement> GetCurrentAccountAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendWithApiKeyAsync(HttpMethod.Get, "api/v2/Account", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await ParseJsonResponse(response, cancellationToken);
    }

    // ================================================================================
    // HELPER METHODS
    // ================================================================================

    private static async Task<JsonElement> ParseJsonResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<JsonElement>(content);
    }
}
