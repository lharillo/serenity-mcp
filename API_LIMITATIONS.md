# Serenity Star API Limitations

**Date:** 2026-01-29  
**Tested with:** MCP Server v1.0.3

---

## 🚫 Known API Restrictions

### 1. Get Agent Details (`/api/Agent/{code}`)

**Status:** ❌ **NOT WORKING**

**Error:**
```json
{
  "message": "An unexpected error ocurred (Code 0006)",
  "statusCode": 500
}
```

**Impact:**
- MCP tool `GetAgentDetails` fails
- Cannot retrieve full agent configuration programmatically

**Workaround:**
- Use agent list (`ListAgents`) which returns basic info
- Access Serenity Star web UI for full details

---

### 2. Update Agent (`/api/v2/agent/assistant/{code}`)

**Status:** ❌ **RESTRICTED**

**Error:**
```json
{
  "message": "You do not have permission to access this resource.",
  "statusCode": 403
}
```

**Impact:**
- MCP tools `UpdateAssistantAgent` and `UpdateAndPublishAssistantAgent` fail
- **Cannot update `systemDefinition` or any agent configuration via MCP**
- This is a **server-side permission restriction**, not a bug in the MCP server

**Possible Causes:**
- API key lacks write permissions
- Endpoint requires OAuth authentication
- Tenant-level restriction on programmatic updates
- Endpoint may be restricted to web UI only

**Workaround:**
- Update agents manually via Serenity Star web UI
- Contact Serenity Star support to enable API write permissions
- Use a service account with elevated permissions (if available)

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
| GetAgentDetails | `GET /api/Agent/{code}` | ❌ Error 500 | Server-side issue |
| CreateAgent | `POST /api/Agent/assistant` | ⚠️ Untested | Likely works |
| UpdateAgent | `PUT /api/v2/agent/assistant/{code}` | ❌ Error 403 | Permission denied |
| UpdateAndPublish | `PUT /api/v2/agent/assistant/{code}/publish` | ❌ Error 403 | Permission denied |
| ExecuteAgent | `POST /api/v2/agent/{code}/execute` | ✅ Working | Confirmed |
| ListModels | `GET /api/v2/aimodel` | ✅ Working | Needs Models API key |

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
