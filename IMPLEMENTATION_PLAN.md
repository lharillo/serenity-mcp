# Serenity MCP - Implementation Plan

**Date:** 2026-01-29  
**Current Version:** 1.0.5  
**Total Available Endpoints:** 75

---

## ✅ Already Implemented (35+ tools)

### Agent Tools (AgentTools.cs)
- ✅ ListAgents
- ✅ GetAgentDetails (FIXED in v1.0.5)
- ✅ CreateAssistantAgent
- ✅ UpdateAssistantAgent
- ✅ UpdateAndPublishAssistantAgent
- ✅ ExecuteAgent
- ✅ CreateConversation
- ✅ CreateConversationInfo
- ✅ GetConversationInfoByVersion
- ✅ GetConversation
- ✅ GetTokenUsage
- ✅ SubmitFeedback
- ✅ DeleteFeedback

### Model Tools (ModelTools.cs)
- ✅ ListModels

### Conversation Tools (ConversationTools.cs)
- ✅ GetConversationMessages
- ✅ UpdateConversationContext
- ✅ DeleteConversation

### Insights Tools (InsightsTools.cs)
- ✅ GetInsightsByAgent
- ✅ GetInsightsByVersion
- ✅ GetInsightsByInstance

### Volatile Knowledge Tools (VolatileKnowledgeTools.cs)
- ✅ UploadVolatileKnowledge
- ✅ UploadVolatileKnowledgeFromUrl
- ✅ GetVolatileKnowledgeById
- ✅ DeleteVolatileKnowledge

### Channel Tools (ChannelTools.cs)
- ✅ GetChannelConfiguration

### Agent Instance Tools (AgentInstanceTools.cs)
- ✅ ListAgentInstances

### Account Tools (AccountTools.cs)
- ✅ GetCurrentUser

---

## 🚀 Priority 1: Agent Version Management (Missing)

**Importance:** HIGH - Core functionality for managing agent versions

### Endpoints to Implement:
1. **GET** `/api/v2/AgentVersion/{agentCode}` - List all versions of an agent
2. **GET** `/api/v2/AgentVersion/{agentCode}/Published` - Get published version
3. **GET** `/api/v2/AgentVersion/{agentCode}/{agentVersion}` - Get specific version
4. **POST** `/api/v2/AgentVersion/{agentCode}/Draft` - Create draft
5. **POST** `/api/v2/AgentVersion/{agentCode}/Draft/{version}` - Create draft from version
6. **PUT** `/api/v2/AgentVersion/{agentCode}/Draft/{version}/Save` - Save draft
7. **PUT** `/api/v2/AgentVersion/{agentCode}/Publish/{version}` - Publish version

### New Tool File:
`Tools/AgentVersionTools.cs`

---

## 🚀 Priority 2: Multiple Agent Types (Missing)

**Importance:** HIGH - Serenity supports 5 agent types, we only support 1

### Agent Types Missing:
1. **Activity Agent** - `/api/v2/agent/activity`
2. **Copilot Agent** - `/api/v2/agent/copilot`
3. **Chat Agent** - `/api/v2/agent/chat`
4. **AI Proxy Agent** - `/api/v2/agent/aiproxy`

### Create/Update endpoints for each:
- POST `/api/v2/agent/{type}` - Create
- PUT `/api/v2/agent/{type}/{code}` - Update
- PUT `/api/v2/agent/{type}/{code}/{versionState}` - Update with version state

### Implementation:
- Extend `AgentTools.cs` or create separate files per type
- Each type has different configuration schema (review docs)

---

## 🚀 Priority 3: Datasets & Tables (Missing)

**Importance:** MEDIUM-HIGH - Data management for agents

### Endpoints to Implement:
1. **GET** `/api/v2/Dataset` - List datasets
2. **POST** `/api/v2/Dataset` - Create dataset
3. **GET** `/api/v2/Dataset/{id}` - Get dataset
4. **PATCH** `/api/v2/Dataset/{id}` - Update dataset
5. **DELETE** `/api/v2/Dataset/{id}` - Delete dataset
6. **POST** `/api/v2/Dataset/{id}/query` - Query dataset
7. **POST** `/api/v2/Dataset/{id}/table` - Create table
8. **PATCH** `/api/v2/Dataset/{id}/table/{tableId}` - Update table
9. **DELETE** `/api/v2/Dataset/{id}/table/{tableId}` - Delete table
10. **PATCH** `/api/v2/Dataset/{id}/table/{tableId}/AppendTable` - Append data
11. **PATCH** `/api/v2/Dataset/{id}/table/{tableId}/ReplaceTable` - Replace data
12. **POST** `/api/v2/Dataset/validation-schema` - Validate schema
13. **POST** `/api/v2/Dataset/{id}/table/{tableId}/validation-schema` - Validate table schema

### New Tool File:
`Tools/DatasetTools.cs`

---

## 🚀 Priority 4: Knowledge Management (Partially Done)

**Importance:** MEDIUM - Already have Volatile Knowledge, missing permanent Knowledge

### Missing Endpoints:
1. **POST** `/api/v2/KnowledgeFile/upload` - Upload knowledge file (permanent)
2. **POST** `/api/v2/KnowledgeFile/upload/{agentCode}` - Upload for specific agent
3. **DELETE** `/api/v2/KnowledgeFile` - Delete knowledge file

### Extend:
`Tools/KnowledgeTools.cs` (merge with VolatileKnowledgeTools or create new)

---

## 🚀 Priority 5: Advanced Features

**Importance:** MEDIUM - Nice to have

### 5a. Embeddings
- **POST** `/api/v2/Embeddings/generate` - Generate embeddings

**New File:** `Tools/EmbeddingsTools.cs`

### 5b. Transcription
- **POST** `/api/v2/Audio/transcribe` - Transcribe audio/video
- **POST** `/api/v2/Audio/transcribe/file` - Transcribe by file ID

**New File:** `Tools/TranscriptionTools.cs`

### 5c. File Management
- **POST** `/api/v2/File/upload` - Upload file
- **GET** `/api/v2/File/{id}` - Display file
- **GET** `/api/v2/File/display/public/{id}` - Display public file
- **GET** `/api/v2/File/download/{id}` - Download file

**New File:** `Tools/FileTools.cs`

### 5d. Conversation Context
- **GET** `/api/v2/agent/{code}/conversation/context` - List conversation context
- **GET** `/api/v2/agent/{code}/{version}/conversation/context` - Context by version
- **GET** `/api/v2/Agent/{code}/conversation/{id}/context` - Single conversation context

**Extend:** `Tools/ConversationTools.cs`

---

## 📋 Implementation Steps

### Phase 1: Fix Current Issues ✅
- [x] v1.0.5 - Fix GetAgentDetails endpoint

### Phase 2: Agent Versions (v1.1.0)
- [ ] Create AgentVersionTools.cs
- [ ] Implement 7 version management tools
- [ ] Test draft/publish workflow
- [ ] Update documentation

### Phase 3: Multiple Agent Types (v1.2.0)
- [ ] Review schema for Activity/Copilot/Chat/AIProxy agents
- [ ] Implement Create tools for each type
- [ ] Implement Update tools for each type
- [ ] Test each agent type creation

### Phase 4: Datasets (v1.3.0)
- [ ] Create DatasetTools.cs
- [ ] Implement dataset CRUD
- [ ] Implement table operations
- [ ] Implement query functionality
- [ ] Test with sample data

### Phase 5: Knowledge & Advanced (v1.4.0)
- [ ] Extend KnowledgeTools
- [ ] Add Embeddings support
- [ ] Add Transcription support
- [ ] Add File management
- [ ] Extend Conversation context tools

---

## 🎯 Success Criteria

- **v1.0.5:** GetAgentDetails works correctly ✅
- **v1.1.0:** Full agent version lifecycle management
- **v1.2.0:** Support all 5 agent types
- **v1.3.0:** Complete dataset management
- **v1.4.0:** All 75 documented endpoints covered

---

## 📊 Coverage Tracking

| Category | Endpoints Available | Implemented | Coverage |
|----------|---------------------|-------------|----------|
| Agents | 40 | 13 | 32% |
| Analytics | 3 | 3 | 100% |
| Conversation | 5 | 3 | 60% |
| Datasets | 13 | 0 | 0% |
| Embeddings | 1 | 0 | 0% |
| Feedback | 2 | 2 | 100% |
| Files | 5 | 0 | 0% |
| Knowledge | 7 | 4 | 57% |
| Models | 1 | 1 | 100% |
| Transcription | 1 | 0 | 0% |
| Other | 6 | 1 | 17% |
| **TOTAL** | **75** | **~27** | **36%** |

---

**Next Steps:**
1. Deploy v1.0.5 with GetAgentDetails fix ✅
2. Review documentation for AgentVersion schema
3. Implement AgentVersionTools.cs
4. Test version management workflow
5. Release v1.1.0
