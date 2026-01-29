# Serenity Star MCP Server

A Model Context Protocol (MCP) server that provides tools for interacting with Serenity Star's AI platform. Built using the official C# MCP SDK.

## About MCP

The Model Context Protocol (MCP) is an open protocol that standardizes how applications provide context to Large Language Models (LLMs). This server enables secure integration between LLMs and Serenity Star's AI agents and models.

## Features

- **MCP Tools**: Execute Serenity Star AI agents
- **Model Discovery**: List available AI models  
- **Agent Management**: Query and interact with configured agents
- **Secure Authentication**: Bearer token validation
- **Standard Protocol**: Fully compliant MCP implementation

## Installation

### Prerequisites
- .NET 8.0 or later
- Serenity Star API access and bearer token

### Build from Source
```bash
git clone https://github.com/lharillo/serenity-mcp.git
cd serenity-mcp
dotnet restore
dotnet build
```

## Configuration

### Environment Variables
```bash
SERENITY_API_KEY=your_serenity_api_key_here
SERENITY_API_URL=https://api.serenitystar.ai  # Default
```

### appsettings.json
```json
{
  "SerenityApi": {
    "ApiKey": "your-api-key-here",
    "BaseUrl": "https://api.serenitystar.ai"
  }
}
```

## Usage

### As Standalone Server
```bash
dotnet run
```

### With MCP Client
Connect any MCP-compatible client to this server using stdio transport.

## Available Tools

### execute_agent
Execute a Serenity Star AI agent with parameters.

**Parameters:**
- `agent_code` (string): The agent identifier
- `message` (string): Message to send to the agent
- `channel` (string, optional): Channel identifier (default: "MCP")
- `user_identifier` (string): User identification

### list_agents  
List all available Serenity Star agents.

### list_models
List all available AI models from Serenity Star.

### get_agent_details
Get detailed information about a specific agent.

**Parameters:**
- `agent_code` (string): The agent identifier

## Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "SerenityStarMcp.dll"]
```

```bash
docker build -t serenity-mcp .
docker run -e SERENITY_API_KEY=your_key serenity-mcp
```

## Development

### Project Structure
```
src/
├── Program.cs              # MCP server setup
├── Tools/
│   ├── AgentTools.cs       # Agent execution tools
│   ├── ModelTools.cs       # Model discovery tools
│   └── ...
├── Services/
│   └── SerenityApiClient.cs # API client service
└── Models/
    └── ...                 # Data models
```

### Adding New Tools

1. Create a new tool class with `[McpServerToolType]`
2. Add methods with `[McpServerTool]` attribute
3. Tools are automatically discovered and registered

```csharp
[McpServerToolType]
public class MyTool
{
    [McpServerTool, Description("My custom tool")]
    public static string MyFunction(string input) => $"Processed: {input}";
}
```

## Security

- API keys should be stored securely (environment variables or key vault)
- This server is designed for trusted environments
- Configure proper access controls in production deployments

## License

Apache License 2.0

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request