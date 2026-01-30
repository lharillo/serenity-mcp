# Serenity Star MCP Server

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![MCP](https://img.shields.io/badge/MCP-v0.7.0-green)](https://modelcontextprotocol.io/)
[![Version](https://img.shields.io/badge/version-1.3.1-blue)](https://github.com/lharillo/serenity-mcp/releases)

Model Context Protocol (MCP) server for the [Serenity Star AI Platform](https://serenitystar.ai), providing **75 tools** with **100% API coverage** for AI agent management, datasets, knowledge, embeddings, transcription, and conversation handling via HTTP/SSE transport.

## 🚀 Quick Start

### VS Code Setup (Recommended)

Use **HTTP Streamable** (the modern MCP transport) for best performance:

```json
{
  "servers": {
    "serenity-star": {
      "type": "http",
      "url": "https://your-mcp-server.example.com/",
      "headers": {
        "X-Serenity-API-Key": "YOUR_API_KEY_HERE"
      }
    }
  }
}
```

**Replace:**
- `your-mcp-server.example.com` with your MCP server URL
- `YOUR_API_KEY_HERE` with your actual Serenity Star API key

**Note:** The trailing `/` in the URL is important - it points to the root endpoint where HTTP Streamable is served.

**👉 See detailed [VS Code Setup Guide](VSCODE_SETUP.md)** for step-by-step instructions, troubleshooting, and alternative configurations (including legacy SSE transport).

### Claude Desktop Configuration

Claude Desktop currently requires the `mcp-remote` proxy for remote servers:

```json
{
  "mcpServers": {
    "serenity-star": {
      "command": "npx",
      "args": [
        "-y",
        "mcp-remote",
        "https://your-mcp-server.example.com/",
        "--header",
        "X-Serenity-API-Key: YOUR_API_KEY_HERE"
      ]
    }
  }
}
```

**Important:** Your Serenity Star API key must be sent via the `X-Serenity-API-Key` header. The server does not store any credentials.

### Local Development

```bash
# Clone repository
git clone https://github.com/lharillo/serenity-mcp.git
cd serenity-mcp

# Build and run
dotnet build
dotnet run

# Server starts on http://localhost:8080
```

## 📡 API Endpoints

The server supports both **SSE** and **HTTP Streamable** transports:

### SSE Transport
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/sse` | Server-Sent Events for MCP protocol |
| `POST` | `/message?sessionId=<id>` | Send messages to MCP server |

### HTTP Streamable Transport
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/` | Send JSON-RPC requests (initialize, tools, etc.) |

### Utility Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/health` | Health check (returns status, timestamp, version) |
| `GET` | `/docs` | Interactive documentation |

**Note:** The server automatically handles both transports. VS Code with `"type": "http"` or `"type": "sse"` will use the appropriate endpoints.

## 🛠️ Available Tools (75 Total)

### Agent Management (33 tools)

#### 5 Agent Types - Complete CRUD
Each agent type supports CREATE, UPDATE, and UPDATE+PUBLISH operations:

**Assistant Agent (3 tools):**
- `CreateAssistantAgent` - Create general purpose conversational agents
- `UpdateAssistantAgent` - Update without publishing
- `UpdateAssistantAgentWithVersion` - Update with version control

**Activity Agent (3 tools):**
- `CreateActivityAgent` - Create workflow automation agents
- `UpdateActivityAgent` - Update without publishing
- `UpdateActivityAgentWithVersion` - Update with version control

**Copilot Agent (3 tools):**
- `CreateCopilotAgent` - Create interactive assistants with real-time suggestions
- `UpdateCopilotAgent` - Update without publishing
- `UpdateCopilotAgentWithVersion` - Update with version control

**Chat Agent (3 tools):**
- `CreateChatAgent` - Create chat completion agents
- `UpdateChatAgent` - Update without publishing
- `UpdateChatAgentWithVersion` - Update with version control

**AI Proxy Agent (3 tools):**
- `CreateAIProxyAgent` - Create direct model access agents (no processing)
- `UpdateAIProxyAgent` - Update without publishing
- `UpdateAIProxyAgentWithVersion` - Update with version control

#### Version Management (8 tools)
- `ListAgentVersions` - List all versions of an agent
- `GetCurrentAgentVersion` - Get current draft version
- `GetPublishedAgentVersion` - Get published version
- `GetAgentVersionByNumber` - Get specific version by number
- `CreateAgentDraft` - Create new draft version
- `CreateAgentDraftFromVersion` - Create draft from existing version
- `SaveDraftVersion` - Save changes to draft
- `PublishAgentVersion` - Publish draft version

#### Basic Operations (8 tools)
- `ListAgents` - List all available agents with pagination
- `GetAgentDetails` - Get detailed information about a specific agent
- `ExecuteAgent` - Execute an agent (supports stateless & stateful with chatId)
- `CreateConversation` - Create a stateful conversation
- `CreateConversationInfo` - Create conversation info with context variables
- `GetConversationInfoByVersion` - Get conversation info for specific version
- `GetConversation` - Get conversation details and history
- `GetTokenUsage` - Get token usage statistics for billing

#### Feedback (2 tools)
- `SubmitFeedback` - Submit RLHF feedback for agent responses
- `DeleteFeedback` - Delete previously submitted feedback

#### Instance Management (1 tool)
- `ListAgentInstances` - List all agent instances for monitoring

### Dataset Management (11 tools)

#### Dataset Operations (6 tools)
- `ListDatasets` - List all datasets with pagination
- `CreateDataset` - Create new dataset
- `GetDataset` - Get dataset details
- `UpdateDataset` - Update dataset metadata
- `DeleteDataset` - Delete dataset
- `QueryDataset` - Query dataset with filters

#### Table Operations (5 tools)
- `CreateTable` - Create table in dataset
- `UpdateTable` - Update table schema/metadata
- `DeleteTable` - Delete table
- `AppendToTable` - Append rows to table
- `ReplaceTableData` - Replace all table data

### Knowledge Management (7 tools)

#### Permanent Knowledge (3 tools)
- `UploadKnowledgeFile` - Upload permanent knowledge files
- `UploadKnowledgeFileForAgent` - Upload knowledge for specific agent
- `DeleteKnowledgeFile` - Delete knowledge files

#### Volatile Knowledge (4 tools)
- `UploadVolatileKnowledge` - Upload temporary knowledge (base64-encoded)
- `UploadVolatileKnowledgeFromUrl` - Upload temporary knowledge from URL
- `GetVolatileKnowledgeById` - Get volatile knowledge details
- `DeleteVolatileKnowledge` - Delete volatile knowledge

### Conversation Management (5 tools)

#### Context Management (3 tools)
- `GetContextList` - List context variables for agent
- `GetContextByVersion` - Get context for specific agent version
- `GetConversationContext` - Get context for specific conversation

#### Variable Updates (2 tools)
- `UpdateContextVariables` - Update conversation context variables
- `DeleteConversation` - Delete conversation and its history

### Advanced Features (9 tools)

#### Embeddings (1 tool)
- `GenerateEmbeddings` - Generate text embeddings for semantic search

#### Transcription (2 tools)
- `TranscribeAudioFile` - Transcribe audio/video files
- `TranscribeAudioByFileId` - Transcribe by file ID

#### File Management (3 tools)
- `UploadFile` - Upload file and get file ID
- `GetFileInfo` - Get file metadata
- `DownloadFile` - Download file (returns base64)

#### Models (1 tool)
- `ListModels` - List all available AI models with UUIDs (essential for agent creation)

#### Analytics (2 tools)
- `GetInsightsByAgent` - Get analytics for an agent
- `GetInsightsByVersion` - Get analytics for a specific version

### Platform Management (10 tools)

#### Account Management (4 tools)
- `GetCurrentUser` - Get current user information
- `LoginUser` - Login and obtain authentication token
- `LogoutUser` - Logout current user
- `RefreshToken` - Refresh authentication token

#### Validation (2 tools)
- `ValidateDatasetSchema` - Validate dataset file schema before upload
- `ValidateTableSchema` - Validate table file schema before upload

#### Subtenants (1 tool)
- `ListSubtenants` - List all subtenants with pagination

#### Channel Configuration (1 tool)
- `GetChannelConfig` - Get channel configuration for an agent

#### Agent Insights (2 tools)
- `GetInsightsByAgentInstance` - Get analytics for agent instance
- `GetInsightsByVersion` - Get analytics for version (duplicate listed above)

## 📊 Coverage Statistics

| Category | Tools | Status |
|----------|-------|--------|
| Agent Management | 33 | ✅ 100% |
| Dataset Management | 11 | ✅ 100% |
| Knowledge Management | 7 | ✅ 100% |
| Conversation Management | 5 | ✅ 100% |
| Advanced Features | 9 | ✅ 100% |
| Platform Management | 10 | ✅ 100% |
| **Total** | **75** | **✅ 100%** |

## 🔐 Security Model

**No credentials stored server-side:**
- API keys are provided by MCP clients via HTTP headers
- Each request includes `X-Serenity-API-Key` header
- Server acts as a transparent proxy to Serenity Star API

**Best practices:**
- Store API keys in your MCP client configuration
- Never commit API keys to version control
- Rotate API keys regularly
- Use environment variables or secure vaults for key management

## 🏗️ Architecture

```
MCP Client (VS Code, Claude Desktop, etc.)
  ↓
  Headers: X-Serenity-API-Key
  ↓
MCP Server (this)
  ↓
  Forward API key + request
  ↓
Serenity Star API (https://api.serenitystar.ai)
```

**Technology Stack:**
- .NET 10.0 with ASP.NET Core
- Official Microsoft MCP SDK (`ModelContextProtocol.AspNetCore` v0.7.0)
- HTTP/SSE transport (Kubernetes-compatible)
- Stateless architecture (no session state stored)
- Docker containerized deployment

## 📋 Configuration

### Base URL

The Serenity Star API base URL can be configured in `appsettings.json`:

```json
{
  "SerenityApi": {
    "BaseUrl": "https://api.serenitystar.ai"
  }
}
```

Default: `https://api.serenitystar.ai`

### Environment Variables

- `ASPNETCORE_URLS` - Server bind URLs (default: `http://+:8080`)
- `SerenityApi__BaseUrl` - Serenity API base URL override

**Note:** API keys are NOT configured as environment variables. They must come from client headers.

## 🧪 Testing

### Health Check
```bash
curl https://your-mcp-server.example.com/health
```

Expected response:
```json
{
  "status": "healthy",
  "timestamp": "2026-01-30T05:52:00.000Z",
  "version": "1.3.1"
}
```

### SSE Connection Test
```bash
curl -N -H "Accept: text/event-stream" \
     -H "X-Serenity-API-Key: YOUR_KEY" \
     https://your-mcp-server.example.com/sse
```

Expected response:
```
event: endpoint
data: /message?sessionId=<unique-session-id>
```

### List Tools Test
```bash
curl -X POST https://your-mcp-server.example.com/ \
  -H "Content-Type: application/json" \
  -H "Mcp-Session-Id: test-session" \
  -H "X-Serenity-API-Key: YOUR_KEY" \
  -d '{"jsonrpc":"2.0","method":"tools/list","id":1}'
```

## 🔧 Development

### Project Structure

```
serenity-mcp/
├── Services/
│   └── SerenityApiClient.cs       # HTTP client for Serenity API (75+ methods)
├── Tools/
│   ├── AccountTools.cs             # Account management (4 tools)
│   ├── AgentInstanceTools.cs       # Instance management (1 tool)
│   ├── AgentTools.cs               # Agent CRUD (13 tools)
│   ├── AgentVersionTools.cs        # Version management (8 tools)
│   ├── ChannelTools.cs             # Channel config (1 tool)
│   ├── ConversationTools.cs        # Conversation management (4 tools)
│   ├── DatasetTools.cs             # Dataset operations (11 tools)
│   ├── EmbeddingsTools.cs          # Embeddings (1 tool)
│   ├── FileTools.cs                # File management (3 tools)
│   ├── InsightsTools.cs            # Analytics (3 tools)
│   ├── KnowledgeTools.cs           # Knowledge management (3 tools)
│   ├── ModelTools.cs               # Model discovery (1 tool)
│   ├── MultiAgentTools.cs          # Multi-agent types (12 tools)
│   ├── SubtenantTools.cs           # Subtenant management (1 tool)
│   ├── TranscriptionTools.cs       # Transcription (2 tools)
│   ├── ValidationTools.cs          # Schema validation (2 tools)
│   └── VolatileKnowledgeTools.cs   # Volatile knowledge (4 tools)
├── Models/                         # Data models and DTOs
├── wwwroot/                        # Static landing page
├── Program.cs                      # Application entry point
├── Version.cs                      # Version information
└── SerenityStarMcp.csproj          # Project configuration
```

### Building

```bash
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run locally
dotnet run

# Build for production
dotnet publish -c Release
```

### Running Tests

```bash
# Run test suite
cd tests
./test-comprehensive.sh
```

## 🚢 Deployment

### Docker

```bash
# Build image
docker build -t serenity-mcp:1.3.1 .

# Run container
docker run -p 8080:8080 serenity-mcp:1.3.1
```

**Docker Hub:** `lharillo/serenity-mcp:1.3.1`

### Kubernetes

Example deployment manifest:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: serenity-mcp
  namespace: default
spec:
  replicas: 1
  selector:
    matchLabels:
      app: serenity-mcp
  template:
    metadata:
      labels:
        app: serenity-mcp
    spec:
      containers:
      - name: serenity-mcp
        image: lharillo/serenity-mcp:1.3.1
        ports:
        - containerPort: 8080
          name: http
        env:
        - name: ASPNETCORE_URLS
          value: "http://+:8080"
        livenessProbe:
          httpGet:
            path: /health
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 30
        readinessProbe:
          httpGet:
            path: /health
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 10
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
---
apiVersion: v1
kind: Service
metadata:
  name: serenity-mcp
  namespace: default
spec:
  selector:
    app: serenity-mcp
  ports:
  - port: 8080
    targetPort: 8080
    name: http
  type: ClusterIP
```

See [k8s/](k8s/) directory for complete Kubernetes manifests.

## 📚 Documentation

- **MCP Protocol:** https://modelcontextprotocol.io
- **Serenity Star API:** https://docs.serenitystar.ai
- **Microsoft .NET MCP SDK:** https://github.com/modelcontextprotocol/csharp-sdk
- **VS Code Setup Guide:** [VSCODE_SETUP.md](VSCODE_SETUP.md)
- **API Limitations:** [API_LIMITATIONS.md](API_LIMITATIONS.md)
- **Implementation Plan:** [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md)
- **Changelog:** [CHANGELOG.md](CHANGELOG.md)

## 📝 Changelog

See [CHANGELOG.md](CHANGELOG.md) for detailed version history.

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

## 📞 Support

- **Documentation:** https://docs.serenitystar.ai
- **Issues:** https://github.com/lharillo/serenity-mcp/issues
- **Discussions:** https://github.com/lharillo/serenity-mcp/discussions
- **Website:** https://subgen.ai

## 🔗 Links

- **GitHub Repository:** https://github.com/lharillo/serenity-mcp
- **Docker Hub:** https://hub.docker.com/r/lharillo/serenity-mcp
- **Releases:** https://github.com/lharillo/serenity-mcp/releases
- **Serenity Star Platform:** https://serenitystar.ai

---

**Built with ❤️ by [Subgen AI](https://subgen.ai)**

**Version:** 1.3.1 | **Status:** Production Ready ✅ | **Coverage:** 100% (75/75 tools)
