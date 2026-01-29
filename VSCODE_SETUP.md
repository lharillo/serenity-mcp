# VS Code Configuration for Serenity Star MCP Server

## Quick Setup (Recommended)

The simplest way to use the Serenity Star MCP Server with VS Code is through direct SSE connection.

### Prerequisites

- VS Code 1.102 or later
- GitHub Copilot extension installed and active
- Valid Serenity Star API key

### Configuration (Direct SSE)

Create or edit `.vscode/mcp.json` in your project:

```json
{
  "servers": {
    "serenity-star": {
      "type": "sse",
      "url": "https://serenitystar-mcp.starkcloud.cc/sse",
      "headers": {
        "X-Serenity-API-Key": "YOUR_API_KEY_HERE",
        "Accept": "text/event-stream"
      }
    }
  }
}
```

**Replace `YOUR_API_KEY_HERE`** with your actual Serenity Star API key.

### Alternative: mcp-remote Proxy (If Direct SSE Fails)

If the direct SSE connection doesn't work, you can use `mcp-remote` as a proxy:

```json
{
  "mcpServers": {
    "serenity-star": {
      "command": "npx",
      "args": [
        "-y",
        "mcp-remote",
        "https://serenitystar-mcp.starkcloud.cc/sse",
        "--header",
        "X-Serenity-API-Key: YOUR_API_KEY_HERE"
      ]
    }
  }
}
```

### Global Configuration

To use the server across all workspaces:

1. Open Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`)
2. Run: `MCP: Add Server`
3. Enter server details:
   - **Type:** Select "Standard Input/Output (stdio)"
   - **Command:** `npx`
   - **Args:** `-y`, `mcp-remote`, `https://serenitystar-mcp.starkcloud.cc/sse`, `--header`, `X-Serenity-API-Key: YOUR_KEY`

## Using the Server

Once configured:

1. **Open Chat View** (`Ctrl+Alt+I` / `Cmd+Alt+I`)
2. **Click the Tools button** (⚙️)
3. **Select tools** from `serenity-star` (35+ available)
4. **Ask questions** like:
   - "List available AI models"
   - "Create a new agent called TestBot"
   - "Show me my conversations"

## Available Tools (35+)

### Agent Management
- `serenity_get_agents` - List all agents
- `serenity_get_agent` - Get specific agent details  
- `serenity_create_agent` - Create new agent
- `serenity_update_agent` - Update existing agent
- `serenity_publish_agent` - Publish agent
- `serenity_execute_agent` - Execute agent with message
- `serenity_execute_agent_stateless` - Execute without conversation

### Model Discovery
- `serenity_list_models` - List 100+ AI models
- `serenity_get_model_by_uuid` - Get model by UUID
- `serenity_search_models` - Search models by name/provider

### Conversations
- `serenity_list_conversations` - List all conversations
- `serenity_get_conversation` - Get conversation details
- `serenity_create_conversation` - Start new conversation
- `serenity_delete_conversation` - Delete conversation
- `serenity_add_message_to_conversation` - Add message

### Document Upload
- `serenity_upload_volatile_knowledge` - Upload temporary documents
- Supports base64-encoded files
- Used for context in agent execution

### Analytics & Insights
- `serenity_get_agent_insights` - Get agent usage analytics
- `serenity_get_token_usage` - View token consumption
- `serenity_submit_feedback` - Submit feedback
- `serenity_get_feedback` - Retrieve feedback

### Account & Channels
- `serenity_get_account_info` - Get account details
- `serenity_list_channels` - List available channels

## Troubleshooting

### Server Not Appearing in VS Code

1. **Reload Window:**
   - Command Palette → `Developer: Reload Window`

2. **Check MCP Server List:**
   - Command Palette → `MCP: List Servers`
   - Look for `serenity-star` in the list

3. **Restart Server:**
   - In server list, click on `serenity-star` → `Restart`

### Connection Errors

1. **Check logs:**
   - View → Output
   - Select "Model Context Protocol" from dropdown

2. **Verify API Key:**
   - Ensure your Serenity Star API key is valid
   - Check at https://docs.serenitystar.ai

3. **Test server directly:**
   ```bash
   curl https://serenitystar-mcp.starkcloud.cc/health
   # Should return: {"status":"healthy",...}
   ```

### "Cannot have more than 128 tools"

VS Code has a 128 tools limit per request. If you hit this:

- Deselect some tools in the Chat view tools picker
- Or ensure virtual tools are enabled (check settings)

### mcp-remote Not Found

Install Node.js if not already installed:
- https://nodejs.org/

The `npx` command will automatically download `mcp-remote` on first use.

## Alternative: Direct SSE Connection (Advanced)

**⚠️ Note:** Direct SSE connection currently has session management issues with the Microsoft MCP SDK. Use the proxy method above for best results.

If you want to try direct SSE anyway:

```json
{
  "mcpServers": {
    "serenity-star": {
      "type": "sse",
      "url": "https://serenitystar-mcp.starkcloud.cc/sse",
      "headers": {
        "X-Serenity-API-Key": "YOUR_API_KEY"
      }
    }
  }
}
```

## Server Information

- **Primary URL:** https://serenitystar-mcp.starkcloud.cc
- **Legacy URL:** https://mcp.starkcloud.cc (still works)
- **Health Check:** https://serenitystar-mcp.starkcloud.cc/health
- **Interactive Docs:** https://serenitystar-mcp.starkcloud.cc/docs
- **Protocol:** MCP 2024-11-05
- **Transport:** HTTP/SSE with mcp-remote proxy
- **Version:** 1.0.2

## Security

- API keys are sent via headers (not stored server-side)
- Server acts as stateless proxy to Serenity API
- HTTPS-only with Cloudflare security
- Non-root container deployment

## Support

- **GitHub:** https://github.com/lharillo/serenity-mcp
- **Issues:** https://github.com/lharillo/serenity-mcp/issues
- **Serenity Docs:** https://docs.serenitystar.ai
- **MCP Protocol:** https://modelcontextprotocol.io

## Example Usage in VS Code

### List Available Models

```
@copilot #serenity_list_models List all available AI models
```

### Create an Agent

```
@copilot #serenity_create_agent Create an agent named "MyBot" with description "A helpful assistant"
```

### Execute Agent

```
@copilot #serenity_execute_agent Execute agent "MyBot" with message "Hello, how are you?"
```

---

**Built by [Subgen AI](https://subgen.ai)** - Enterprise AI Solutions
