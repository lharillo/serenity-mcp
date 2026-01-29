# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2026-01-29

### Added - Agent Version Management (7 new tools)
- **ListAgentVersions** - List all versions with pagination and filtering
- **GetCurrentAgentVersion** - Get current (latest saved) version
- **GetPublishedAgentVersion** - Get published (active) version
- **GetAgentVersionByNumber** - Get specific version by number
- **CreateAgentDraft** - Create draft from current version
- **CreateAgentDraftFromVersion** - Create draft from specific version
- **SaveDraftVersion** - Save draft as normal version
- **PublishAgentVersion** - Publish version as active

### Added - Dataset Management (11 new tools)
- **ListDatasets** - List all datasets with pagination
- **CreateDataset** - Create new dataset
- **GetDataset** - Get dataset by ID
- **UpdateDataset** - Update dataset info
- **DeleteDataset** - Delete dataset
- **QueryDataset** - Query with natural language or SQL
- **CreateTable** - Create table in dataset
- **UpdateTable** - Update table structure
- **DeleteTable** - Delete table
- **AppendToTable** - Append data to table
- **ReplaceTableData** - Replace all table data

### Added - Embeddings (1 new tool)
- **GenerateEmbeddings** - Generate embeddings for text

### Added - Transcription (2 new tools)
- **TranscribeAudioFile** - Transcribe audio/video file
- **TranscribeAudioByFileId** - Transcribe by file ID

### Added - File Management (3 new tools)
- **UploadFile** - Upload files to Serenity Star
- **GetFileInfo** - Get file metadata
- **DownloadFile** - Download file by ID

### Added - New Tool Files
- `Tools/AgentVersionTools.cs` - Version management
- `Tools/DatasetTools.cs` - Dataset & table operations
- `Tools/EmbeddingsTools.cs` - Embedding generation
- `Tools/TranscriptionTools.cs` - Audio/video transcription
- `Tools/FileTools.cs` - File operations

### Coverage Increase
- **Before:** 27 tools (36% of API)
- **After:** 51 tools (68% of API)
- **Added:** 24 new tools

### Technical
- All new endpoints tested against API documentation
- Comprehensive error handling
- Support for multipart file uploads
- PATCH method support for datasets

## [1.0.4] - 2026-01-29

### Fixed
- **UpdateAssistantAgent** - Now uses proper camelCase structure with all required fields
- **UpdateAndPublishAssistantAgent** - Same fixes as Update tool
- **GetAgentDetails** - Implemented workaround for broken API endpoint (filters from list)

### Changed
- Update tools now accept individual parameters instead of JSON string
- Proper field mapping: PascalCase for Create, camelCase for Update
- `knowledge` field always included (required by API, even if empty)

### Tested
- ✅ Update works with demo API key (tested and confirmed)
- ✅ Execute agent works (stateful and stateless)
- ✅ List models works
- ✅ GetAgentDetails workaround provides basic info
- ❌ Original GetAgentDetails endpoint broken (API returns 500)

### Documentation
- Updated API_LIMITATIONS.md with correct testing results
- Added proper Update structure documentation
- Clarified permission requirements for write operations

### Technical
- Tools tested against production and demo Serenity Star APIs
- Fixed case sensitivity issues in request payloads
- All major tools confirmed working with proper permissions

## [1.0.3] - 2026-01-29

### Changed
- **BREAKING:** URL changed to `serenitystar-mcp.starkcloud.cc`
- Removed PathBase middleware completely (now at root path)
- **HTTP Streamable is now the recommended primary transport** (confirmed working in VS Code 1.102+)
- Updated all documentation with modern HTTP Streamable configuration
- Fixed health check endpoints (removed `/serenitystar` prefix)

### Added
- **HTTP Streamable configuration** as primary method (`type: "http"`, URL: root `/`)
- Direct SSE configuration as alternative/legacy option
- `vscode-config-http.json` - HTTP Streamable config example
- `vscode-config-sse.json` - SSE config example (legacy)
- Comprehensive troubleshooting guide in VSCODE_SETUP.md
- Cloudflare HTTP/2 origin support for optimal performance
- Dedicated subdomain: serenitystar-mcp.starkcloud.cc

### Fixed
- Health probe paths in Kubernetes deployment (`/health` instead of `/serenitystar/health`)
- Connection stability with Cloudflare Tunnel (HTTP/2 origin, disabled chunked encoding)
- Documentation now reflects working configurations confirmed with VS Code 1.102+

### Technical
- Version: 1.0.3
- **Both SSE and HTTP Streamable transports fully supported**
- `POST /` for HTTP Streamable (modern, recommended)
- `GET /sse` for SSE transport (legacy, still supported)
- Microsoft MCP SDK `WithHttpTransport()` registers both endpoints automatically

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
