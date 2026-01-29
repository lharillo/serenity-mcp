# Serenity Star MCP Server

Official Model Context Protocol (MCP) server for [Serenity Star AI Platform](https://serenitystar.ai).

## Quick Start

```bash
docker pull lharillo/serenity-mcp:latest
```

## What is this?

This is an MCP server that provides 35+ tools for managing Serenity Star AI agents, models, conversations, and analytics. It acts as a bridge between MCP clients (like Claude Desktop, VS Code) and the Serenity Star API.

## Features

- ✅ **35+ Tools** - Complete CRUD for agents, models, conversations, document upload, analytics
- ✅ **Header-Based Auth** - Secure API key authentication via HTTP headers
- ✅ **HTTP/SSE Transport** - Real-time Server-Sent Events
- ✅ **Stateless** - No credentials stored server-side
- ✅ **Production Ready** - Built with .NET 10, official Microsoft MCP SDK

## Running the Server

### Basic Usage

```bash
docker run -d \
  -p 8080:8080 \
  --name serenity-mcp \
  lharillo/serenity-mcp:latest
```

The server will be available at `http://localhost:8080`

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_URLS` | Server binding URLs | `http://+:8080` |
| `SerenityApi__BaseUrl` | Serenity API base URL | `https://api.serenitystar.ai` |

**Note:** API keys are NOT configured as environment variables. They must be provided by MCP clients via the `X-Serenity-API-Key` header.

## MCP Client Configuration

Configure your MCP client to connect to the server:

```json
{
  "mcpServers": {
    "serenity-star": {
      "url": "http://localhost:8080/sse",
      "headers": {
        "X-Serenity-API-Key": "YOUR_SERENITY_STAR_API_KEY"
      }
    }
  }
}
```

## Available Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/sse` | GET | Server-Sent Events (MCP protocol) |
| `/message?sessionId=<id>` | POST | Send messages to MCP server |
| `/health` | GET | Health check |
| `/docs` | GET | Interactive documentation |

## Available Tools

### Agent Management
- Create, update, publish agents
- Execute agents with messages
- Manage conversations & context
- Token usage & analytics

### Model Discovery
- List 100+ available models
- Get model UUIDs & capabilities
- GPT-4, Claude, Gemini, and more

### Document Upload
- Upload volatile knowledge
- Temporary document context for agents
- Base64-encoded file support

### Analytics & Feedback
- Submit & manage feedback
- Token usage statistics
- Agent insights & metrics

## Security

**No credentials stored server-side:**
- API keys provided by MCP clients via `X-Serenity-API-Key` header
- Server acts as transparent proxy to Serenity Star API
- All requests require valid API key

## Architecture

```
MCP Client
  ↓ (Headers: X-Serenity-API-Key)
  ↓
MCP Server (this container)
  ↓ (Forward API key)
  ↓
Serenity Star API
```

## Image Tags

- `latest` - Latest stable release
- `1.0.0` - Specific version
- `1.0` - Minor version
- `1` - Major version

## Health Check

```bash
curl http://localhost:8080/health
```

Expected response:
```json
{
  "status": "healthy",
  "timestamp": "2026-01-29T19:00:00Z",
  "version": "1.0.0"
}
```

## Documentation

- **Live Demo:** https://mcp.starkcloud.cc/serenitystar/docs
- **GitHub:** https://github.com/subgenai/serenity-mcp
- **Serenity Star Docs:** https://docs.serenitystar.ai
- **MCP Protocol:** https://modelcontextprotocol.io

## Support

- **Issues:** GitHub Issues
- **Documentation:** https://docs.serenitystar.ai
- **Website:** https://subgen.ai

## License

MIT License - See [LICENSE](https://github.com/subgenai/serenity-mcp/blob/main/LICENSE)

---

**Built by [Subgen AI](https://subgen.ai)** - Enterprise AI Solutions
