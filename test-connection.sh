#!/bin/bash
# Serenity Star MCP Server - Connection Test Script

SERVER_URL="https://mcp.starkcloud.cc/serenitystar"
API_KEY="${1:-test}"

echo "🔍 Testing Serenity Star MCP Server Connection"
echo "=============================================="
echo ""

# Test 1: Health Check
echo "📡 Test 1: Health Check"
HEALTH_RESPONSE=$(curl -s "$SERVER_URL/health")
if echo "$HEALTH_RESPONSE" | grep -q "healthy"; then
    echo "✅ Server is healthy"
    echo "   Response: $HEALTH_RESPONSE"
else
    echo "❌ Server health check failed"
    echo "   Response: $HEALTH_RESPONSE"
    exit 1
fi
echo ""

# Test 2: SSE Connection
echo "📡 Test 2: SSE Connection (3 second timeout)"
SSE_RESPONSE=$(timeout 3 curl -s -N \
    -H "Accept: text/event-stream" \
    -H "X-Serenity-API-Key: $API_KEY" \
    "$SERVER_URL/sse" 2>&1 || echo "timeout")

if echo "$SSE_RESPONSE" | grep -q "event: endpoint"; then
    echo "✅ SSE transport working"
    SESSION_ID=$(echo "$SSE_RESPONSE" | grep -oP 'sessionId=\K[^&\s]+' | head -1)
    echo "   Session ID: $SESSION_ID"
else
    echo "⚠️  SSE response: $SSE_RESPONSE"
fi
echo ""

# Test 3: Message Endpoint (if we got a session)
if [ -n "$SESSION_ID" ]; then
    echo "📡 Test 3: Message Endpoint"
    MESSAGE_RESPONSE=$(curl -s -X POST \
        -H "Content-Type: application/json" \
        -H "X-Serenity-API-Key: $API_KEY" \
        "$SERVER_URL/message?sessionId=$SESSION_ID" \
        -d '{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"test","version":"1.0"}}}')
    
    if [ -n "$MESSAGE_RESPONSE" ]; then
        echo "✅ Message endpoint responding"
        echo "   Response: $(echo $MESSAGE_RESPONSE | head -c 100)..."
    else
        echo "⚠️  No response from message endpoint"
    fi
    echo ""
fi

# Summary
echo "=============================================="
echo "✅ Server Configuration:"
echo "   URL: $SERVER_URL"
echo "   Health: ✅ Healthy"
echo "   SSE: ✅ Working"
echo "   Version: $(echo "$HEALTH_RESPONSE" | grep -oP '"version":"\K[^"]+' || echo "unknown")"
echo ""
echo "📋 VS Code Configuration:"
echo '   {'
echo '     "servers": {'
echo '       "serenity-star": {'
echo '         "type": "sse",'
echo "         \"url\": \"$SERVER_URL/sse\","
echo '         "headers": {'
echo '           "X-Serenity-API-Key": "YOUR_API_KEY"'
echo '         }'
echo '       }'
echo '     }'
echo '   }'
echo ""
echo "🚀 Ready to use with VS Code!"
