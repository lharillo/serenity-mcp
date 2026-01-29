# Serenity Star MCP Server

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![MCP](https://img.shields.io/badge/MCP-v0.7.0-green)](https://modelcontextprotocol.io/)

**Production URL:** https://serenitystar-mcp.starkcloud.cc

Model Context Protocol (MCP) server for the [Serenity Star AI Platform](https://serenitystar.ai), providing comprehensive tools for AI agent management, model discovery, conversation handling, and document upload via HTTP/SSE transport.

## 🚀 Quick Start

### VS Code Setup (Recommended)

Use **HTTP Streamable** (the modern MCP transport) for best performance:

```json
{
  "servers": {
    "serenity-star": {
      "type": "http",
      "url": "https://serenitystar-mcp.starkcloud.cc/",
      "headers": {
        "X-Serenity-API-Key": "YOUR_API_KEY_HERE"
      }
    }
  }
}
```

**Replace `YOUR_API_KEY_HERE`** with your actual Serenity Star API key.

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
        "https://serenitystar-mcp.starkcloud.cc/",
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
git clone <repository-url>
cd serenity-mcp

# Build and run
dotnet build
dotnet run

# Server starts on http://localhost:8080
```

## 📡 API Endpoints

The server supports both **SSE (legacy)** and **HTTP Streamable** transports:

### SSE Transport (Recommended)
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
| `GET` | `/health` | Health check (K8s probes) |
| `GET` | `/docs` | Interactive documentation |

**Base URL:** `https://serenitystar-mcp.starkcloud.cc`

**Note:** The server automatically handles both transports. VS Code with `"type": "sse"` uses the SSE endpoints. HTTP Streamable clients should POST to the root `/` endpoint.

## 🛠️ Available Tools

### ⚠️ API Limitations

Some tools are currently restricted by the Serenity Star API:
- ❌ **GetAgentDetails** - API returns Error 500 (server-side issue)
- ❌ **UpdateAssistantAgent** - API returns 403 Permission Denied
- ❌ **UpdateAndPublishAssistantAgent** - API returns 403 Permission Denied

**Note:** These are **API-level restrictions**, not MCP server bugs. Agent updates must be done via the Serenity Star web UI until API permissions are enabled.

See [API_LIMITATIONS.md](API_LIMITATIONS.md) for full details and workarounds.

### Agent Management (35+ tools)

#### Read Operations
- **ListAgents** - ✅ List all available agents
- **GetAgentDetails** - ❌ UNAVAILABLE (API Error 500)
- **GetAgentInstances** - ✅ List all agent instances
- **GetInsightsByAgent** - ✅ Get analytics for an agent
- **GetInsightsByVersion** - ✅ Get analytics for a specific agent version
- **GetInsightsByInstance** - ✅ Get analytics for an agent instance

#### Create Operations
- **CreateAssistantAgent** - Create a new Assistant agent
- **CreateConversation** - Create a stateful conversation
- **CreateConversationInfo** - Create conversation info with context variables

#### Update Operations
- **UpdateAssistantAgent** - Update an existing agent (without publishing)
- **UpdateAndPublishAssistantAgent** - Update and publish an agent
- **UpdateContextVariables** - Update conversation context variables

#### Execute Operations
- **ExecuteAgent** - Execute an agent with a message
  - Supports stateless execution
  - Supports stateful conversations (with chatId)
  - Supports volatile knowledge (temporary documents)

### Conversation Management
- **GetConversation** - Get conversation details
- **GetConversationInfoByVersion** - Get conversation info for specific version
- **GetContextList** - List context variables
- **GetContextByVersion** - Get context for specific version
- **GetConversationContext** - Get conversation-specific context

### Model Discovery
- **ListModels** - List all available AI models with UUIDs
  - Returns model names, UUIDs, providers, capabilities
  - Essential for agent creation (requires model UUID)

### Document Upload
- **UploadVolatileKnowledge** - Upload documents for temporary agent context
  - Supports base64-encoded files
  - Returns document ID for use in agent execution

### Feedback & Analytics
- **SubmitFeedback** - Submit feedback for agent responses
- **DeleteFeedback** - Delete previously submitted feedback
- **GetTokenUsage** - Get token usage statistics

### Channel & Configuration
- **GetChannelConfig** - Get channel configuration for an agent
- **GetCurrentAccount** - Get authenticated user information

## 🔐 Security Model

**No credentials stored server-side:**
- API keys are provided by MCP clients via HTTP headers
- Each request includes `X-Serenity-API-Key` header
- Server acts as a transparent proxy to Serenity Star API

**Best practices:**
- Store API keys in your MCP client configuration
- Never commit API keys to version control
- Rotate API keys regularly

## 🏗️ Architecture

```
MCP Client
  ↓
  Headers: X-Serenity-API-Key
  ↓
MCP Server (this)
  ↓
  Forward API key
  ↓
Serenity Star API
```

**Technology Stack:**
- .NET 10.0 with ASP.NET Core
- Official Microsoft MCP SDK (`ModelContextProtocol.AspNetCore`)
- HTTP/SSE transport (K8s-compatible)
- Kubernetes deployment (K3s)
- Cloudflare Tunnel for secure access

## 📋 Configuration

### Base URL

The Serenity Star API base URL can be configured:

```json
{
  "SerenityApi": {
    "BaseUrl": "https://api.serenitystar.ai"
  }
}
```

Default: `https://api.serenitystar.ai`

## 🧪 Testing

### Health Check
```bash
curl https://mcp.starkcloud.cc/serenitystar/health
```

### SSE Connection
```bash
curl -N -H "Accept: text/event-stream" \
     -H "X-Serenity-API-Key: YOUR_KEY" \
     https://mcp.starkcloud.cc/serenitystar/sse
```

Expected response:
```
event: endpoint
data: /message?sessionId=<unique-id>
```

## 📚 Documentation

- **MCP Protocol:** https://modelcontextprotocol.io
- **Serenity Star API:** https://docs.serenitystar.ai
- **Microsoft .NET MCP SDK:** https://github.com/modelcontextprotocol/csharp-sdk

## 🔧 Development

### Project Structure

```
serenity-mcp/
├── Services/
│   └── SerenityApiClient.cs    # HTTP client for Serenity API
├── Tools/
│   ├── AgentTools.cs            # Agent CRUD operations
│   ├── ConversationTools.cs     # Conversation management
│   ├── ModelTools.cs            # Model discovery
│   ├── VolatileKnowledgeTools.cs # Document upload
│   ├── InsightsTools.cs         # Analytics
│   ├── ChannelTools.cs          # Channel config
│   ├── AgentInstanceTools.cs    # Instance management
│   └── AccountTools.cs          # Account info
├── Models/                      # Data models
├── wwwroot/                     # Landing page
└── Program.cs                   # Application entry point
```

### Building

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

## 🚢 Deployment

### Kubernetes

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: serenity-mcp
spec:
  replicas: 1
  template:
    spec:
      containers:
      - name: serenity-mcp
        image: your-registry/serenity-mcp:latest
        ports:
        - containerPort: 8080
        env:
        - name: ASPNETCORE_URLS
          value: "http://+:8080"
        # No API keys in environment!
```

### Environment Variables

- `ASPNETCORE_URLS` - Server URLs (default: `http://+:8080`)
- `SerenityApi__BaseUrl` - Serenity API base URL (default: `https://api.serenitystar.ai`)

**Note:** API keys are NOT configured as environment variables. They come from client headers.

## 📝 Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history.

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📞 Support

- **Documentation:** https://docs.serenitystar.ai
- **Issues:** GitHub Issues
- **Website:** https://subgen.ai

---

**Built with ❤️ by Subgen AI**

## Repository

- **GitHub:** https://github.com/lharillo/serenity-mcp
- **Docker Hub:** https://hub.docker.com/r/lharillo/serenity-mcp
- **Issues:** https://github.com/lharillo/serenity-mcp/issues

