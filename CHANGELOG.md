# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.3] - 2026-01-29

### Changed
- **BREAKING:** URL changed to `serenitystar-mcp.starkcloud.cc`
- Removed PathBase middleware completely (now at root path)
- Switched to direct SSE configuration as primary method (works natively in VS Code)
- Updated all documentation with working SSE configuration
- Fixed health check endpoints (removed `/serenitystar` prefix)

### Added
- Direct SSE configuration examples for VS Code (`.vscode/mcp.json` format)
- Alternative mcp-remote proxy configuration (fallback option)
- Comprehensive troubleshooting guide in VSCODE_SETUP.md
- Cloudflare HTTP/2 origin support for SSE optimization
- Dedicated subdomain: serenitystar-mcp.starkcloud.cc
- `vscode-config-sse.json` example file

### Fixed
- Health probe paths in Kubernetes deployment (`/health` instead of `/serenitystar/health`)
- SSE connection stability with Cloudflare Tunnel (HTTP/2 origin, disabled chunked encoding)
- Documentation now reflects working configurations confirmed with VS Code 1.102+

### Technical
- Version: 1.0.3
- Both SSE and HTTP Streamable transports supported (MapMcp registers both)
- POST / for HTTP Streamable, GET /sse for SSE transport
- 405 on POST /sse is expected (use POST / for Streamable)

## [1.0.2] - 2026-01-29

### Fixed
- Removed PathBase completely to attempt session issue resolution
- Updated Cloudflare configuration

## [1.0.1] - 2026-01-29

### Fixed
- Simplified PathBase middleware
- Session management investigation

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
