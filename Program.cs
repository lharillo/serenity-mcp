using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using SerenityStarMcp.Services;

var builder = Host.CreateApplicationBuilder(args);

// Configure logging to stderr for MCP protocol compliance
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Add HTTP client for Serenity Star API
builder.Services.AddHttpClient<SerenityApiClient>(client =>
{
    var apiUrl = builder.Configuration["SerenityApi:BaseUrl"] ?? "https://api.serenitystar.ai";
    client.BaseAddress = new Uri(apiUrl);
});

// Configure MCP server with stdio transport and auto-discovered tools
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(); // Automatically discovers all [McpServerTool] methods

// Run the MCP server
await builder.Build().RunAsync();