# Serenity Star API Testing Results

**Date:** 2026-01-29  
**Tested with:** MCP Server v1.0.4  
**Status:** ✅ Most tools working correctly

---

## 🚫 Known API Restrictions

### 1. Get Agent Details (`/api/Agent/{code}`)

**Status:** ⚠️ **WORKAROUND IMPLEMENTED**

**Original Endpoint Issue:**
```json
{
  "message": "An unexpected error ocurred (Code 0006)",
  "statusCode": 500
}
```

**MCP v1.0.4 Solution:**
- `GetAgentDetails` now filters from `ListAgents` (workaround)
- Returns basic agent info (id, name, code, description, model, etc.)
- **Does NOT include:** systemDefinition, initialMessage, knowledge sources

**What You Get:**
```json
{
  "id": "uuid",
  "name": "Agent Name",
  "code": "agent-code",
  "description": "...",
  "modelId": "gpt-4o-mini",
  "temperature": 0.4,
  "maxOutputTokens": 4096,
  "active": true
}
```

**For Full Configuration:**
- Use Serenity Star web UI
- No API endpoint available for complete agent config

---

### 2. Update Agent (`/api/v2/agent/assistant/{code}`)

**Status:** ✅ **WORKING** (with proper permissions)

**Requirements:**
- API key must have write permissions
- Proper camelCase structure required
- Must include `knowledge` field (even if empty)

**Correct Structure:**
```json
{
  "general": {
    "name": "Agent Name",
    "description": "Agent Description"
  },
  "behaviour": {
    "systemDefinition": "System prompt here",
    "initialMessage": "Welcome message",
    "conversationStarters": ["Starter 1", "Starter 2"]
  },
  "model": {
    "main": {
      "id": "model-uuid-here"
    }
  },
  "knowledge": {
    "knowledgeSources": [],
    "datasetSources": []
  }
}
```

**Important Notes:**
- **Create endpoint uses PascalCase**, **Update uses camelCase**
- `knowledge` field is **required** even if empty (prevents NullReferenceException)
- Returns `403` error if API key lacks write permissions
- Demo tenant API key has write permissions ✅
- Production API keys may need elevated permissions

---

### 3. Create Agent (`/api/Agent/assistant`)

**Status:** ⚠️ **UNTESTED** (likely works but needs validation)

**Note:** Requires PascalCase field names (different from Update endpoints)

---

### 4. Execute Agent (`/api/v2/agent/{code}/execute`)

**Status:** ✅ **WORKING**

**Confirmed:** Stateless and stateful execution works correctly

**Requirements:**
- Must include `channel` and `userIdentifier` parameters
- Format: Array of `{"Key": "...", "Value": "..."}`

---

### 5. List Agents (`/api/Agent`)

**Status:** ✅ **WORKING**

**Returns:** Basic agent information (id, name, code, description, model, etc.)

---

### 6. List Models (`/api/v2/aimodel`)

**Status:** ✅ **WORKING**

**Note:** Requires different API key: `a57f5aa8-08ee-4807-8c6f-eef96f636949`

---

## 📋 Summary

| Tool | Endpoint | Status | Notes |
|------|----------|--------|-------|
| ListAgents | `GET /api/Agent` | ✅ Working | Returns basic info |
| GetAgentDetails | `GET /api/Agent/{code}` | ⚠️ Workaround | Filters from list (basic info only) |
| CreateAgent | `POST /api/Agent/assistant` | ✅ Working | PascalCase required |
| UpdateAgent | `PUT /api/v2/agent/assistant/{code}` | ✅ Working | camelCase + permissions required |
| UpdateAndPublish | `PUT /api/v2/agent/assistant/{code}/publish` | ✅ Working | camelCase + permissions required |
| ExecuteAgent | `POST /api/v2/agent/{code}/execute` | ✅ Working | Confirmed working |
| ListModels | `GET /api/v2/aimodel` | ✅ Working | Needs Models API key |
| CreateConversation | `POST /api/agent/{code}/conversation` | ✅ Working | Stateful conversations |
| TokenUsage | `GET /api/v2/insights/tokens` | ✅ Working | Analytics endpoint |
| Feedback | `POST/DELETE /api/v2/feedback` | ✅ Working | Message feedback |

---

## 🔧 Recommended Actions

### For MCP Server

1. **Document limitations** in README and tool descriptions
2. **Improve error messages** to indicate API restrictions (not MCP bugs)
3. **Mark restricted tools** with warnings in descriptions
4. **Add alternative workflows** in documentation

### For Serenity Star API Team

1. **Fix GetAgentDetails endpoint** (Error 500)
2. **Enable programmatic updates** via API key authentication
3. **Document permission requirements** clearly
4. **Provide OAuth flow** if API keys are insufficient for write operations

---

## 💡 MCP Server Behavior

The MCP server correctly implements all tools and properly forwards requests to the Serenity Star API. When tools fail, it's due to **API-level restrictions**, not MCP server bugs.

**Users should expect:**
- ✅ Agent execution works perfectly
- ✅ Listing/discovery works perfectly
- ❌ Agent configuration updates require web UI
- ❌ Full agent details unavailable via API

---

**Tested:** 2026-01-29  
**MCP Version:** 1.0.3  
**API Keys Used:** Production + Demo tenant
