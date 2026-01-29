# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-29

### Added
- Initial release of Serenity Star MCP Server
- HTTP/SSE transport support via official Microsoft MCP SDK
- 35+ tools for Serenity Star AI Platform:
  - **Agent CRUD:** Create, update, publish, list, get details
  - **Agent Execution:** Execute with messages, stateful conversations, volatile knowledge
  - **Model Discovery:** List 100+ models with UUIDs and capabilities
  - **Conversation Management:** Create, list, get, manage context variables
  - **Document Upload:** Volatile knowledge for temporary agent context
  - **Analytics:** Token usage, insights, feedback
  - **Configuration:** Channel config, agent instances, account info
- Interactive landing page with API documentation
- Health check endpoint for Kubernetes monitoring
- Path-based routing support (`/serenitystar` prefix)
- Multi-architecture Docker support (amd64, arm64)
- Comprehensive logging and error handling
- Environment-based configuration (API keys, base URL)
- Static file serving for documentation
- Semantic versioning support

### Technical Details
- Built with .NET 10.0 and ASP.NET Core
- Uses ModelContextProtocol.AspNetCore (v0.7.0-preview.1)
- Deployed on Kubernetes (K3s)
- Secured via Cloudflare Tunnel
- Published to Docker Hub: `lharillo/serenity-mcp`

### Security
- **Header-based API key authentication** - API keys provided by MCP clients via `X-Serenity-API-Key` header
- **No credentials stored server-side** - Server acts as transparent proxy
- HTTPS-only public endpoints via Cloudflare Tunnel
- Non-root container user (appuser:1001)
- Cloudflare security layer

[1.0.0]: https://github.com/subgenai/serenity-mcp/releases/tag/v1.0.0
