# VS Code Configuration for Serenity Star MCP Server

## Server Capabilities

This server uses Microsoft's MCP SDK `WithHttpTransport()` which **automatically supports BOTH**:

- ✅ **HTTP Streamable** (modern, preferred by VS Code)
- ✅ **Server-Sent Events (SSE)** (legacy, fallback)

## VS Code Configuration

### Option 1: Workspace Configuration (Recommended)

Create `.vscode/mcp.json` in your project:

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "serenity-api-key",
      "description": "Serenity Star API Key",
      "password": true
    }
  ],
  "servers": {
    "serenity-star": {
      "type": "http",
      "url": "https://mcp.starkcloud.cc/serenitystar/sse",
      "headers": {
        "X-Serenity-API-Key": "${input:serenity-api-key}"
      }
    }
  }
}
```

### Option 2: Force SSE Transport (If HTTP fails)

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "serenity-api-key",
      "description": "Serenity Star API Key",
      "password": true
    }
  ],
  "servers": {
    "serenity-star": {
      "type": "sse",
      "url": "https://mcp.starkcloud.cc/serenitystar/sse",
      "headers": {
        "X-Serenity-API-Key": "${input:serenity-api-key}"
      }
    }
  }
}
```

## Prerequisites

- VS Code 1.102 or later
- GitHub Copilot extension installed and active
- Valid Serenity Star API key

## Setup Steps

1. **Install/Update VS Code**
   - Ensure you have VS Code 1.102+
   - Update GitHub Copilot extension to latest

2. **Add MCP Server**
   - Copy the configuration above to `.vscode/mcp.json` OR
   - Use Command Palette: `MCP: Add Server`

3. **Provide API Key**
   - VS Code will prompt for your Serenity Star API key
   - The key is stored securely for subsequent uses

4. **Verify Connection**
   - Open Command Palette: `MCP: List Servers`
   - You should see `serenity-star` listed
   - Check status - should show "Running"

5. **Use Tools in Chat**
   - Open Chat view (`Ctrl+Alt+I` / `Cmd+Alt+I`)
   - Click **Tools** button
   - Select tools from `serenity-star` (35+ available)
   - Ask questions like "List available AI models"

## Troubleshooting

### Server Shows "Loading..." Forever

**Cause:** VS Code is trying HTTP Streamable but getting SSE responses

**Fix:** Force SSE transport by using `"type": "sse"` in config (Option 2 above)

### No Response from Server

1. **Check server health:**
   ```bash
   curl https://mcp.starkcloud.cc/serenitystar/health
   ```
   Should return: `{"status":"healthy","timestamp":"...","version":"1.0.0"}`

2. **Test SSE connection:**
   ```bash
   curl -N -H "Accept: text/event-stream" \
        -H "X-Serenity-API-Key: YOUR_KEY" \
        https://mcp.starkcloud.cc/serenitystar/sse
   ```
   Should return SSE stream with endpoint info

3. **Check VS Code logs:**
   - View → Output
   - Select "Model Context Protocol" from dropdown
   - Look for connection errors

### "Cannot have more than 128 tools"

**Cause:** Model constraint limits total tools to 128

**Fix:** Deselect some tools or servers in the Chat view tools picker

### API Key Not Accepted

1. Verify your API key is valid at https://docs.serenitystar.ai
2. Check that the key has appropriate permissions
3. Try regenerating the key if it's expired

## Available Tools (35+)

### Agent Management
- `serenity_get_agents` - List all agents
- `serenity_get_agent` - Get specific agent details
- `serenity_create_agent` - Create new agent
- `serenity_update_agent` - Update existing agent
- `serenity_publish_agent` - Publish agent
- `serenity_execute_agent` - Execute agent with message

### Model Discovery
- `serenity_list_models` - List 100+ AI models
- `serenity_get_model_by_uuid` - Get model by UUID
- `serenity_search_models` - Search models

### Conversations
- `serenity_list_conversations` - List conversations
- `serenity_get_conversation` - Get conversation details
- `serenity_create_conversation` - Create new conversation
- `serenity_delete_conversation` - Delete conversation

### Analytics & More
- Agent insights
- Token usage
- Feedback management
- Document upload

## Support

- **Live Demo:** https://mcp.starkcloud.cc/serenitystar/docs
- **GitHub:** https://github.com/lharillo/serenity-mcp
- **Issues:** https://github.com/lharillo/serenity-mcp/issues
- **Serenity Docs:** https://docs.serenitystar.ai

## Technical Details

- **Transport:** HTTP/SSE (automatic negotiation)
- **Protocol Version:** 2024-11-05
- **SDK:** Microsoft MCP SDK 0.7.0-preview.1
- **Runtime:** .NET 10
- **Deployment:** Kubernetes + Cloudflare Tunnel
- **Status:** ✅ Production (v1.0.0)
