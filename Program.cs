using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using SerenityStarMcp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add HTTP context accessor for reading request headers
builder.Services.AddHttpContextAccessor();

// Configure static files
builder.Services.AddDirectoryBrowser();

// Add HTTP client for Serenity Star API
builder.Services.AddHttpClient<SerenityApiClient>(client =>
{
    var apiUrl = builder.Configuration["SerenityApi:BaseUrl"] ?? "https://api.serenitystar.ai";
    client.BaseAddress = new Uri(apiUrl);
});

// Add MCP Server with HTTP/SSE Transport
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// PROOF OF LIFE: SSE VERSION IS RUNNING
Console.WriteLine("====================================");
Console.WriteLine("🚀 SERENITY MCP SERVER WITH SSE/HTTP");
Console.WriteLine($"   Version: {SerenityStarMcp.Version.FullVersion}");
Console.WriteLine($"   Build: {SerenityStarMcp.Version.BuildTime:yyyy-MM-dd HH:mm:ss} UTC");
Console.WriteLine("====================================");

// Use PathBase for /serenitystar prefix
app.UsePathBase("/serenitystar");

// Health check for K8s (before MCP)
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow, version = SerenityStarMcp.Version.FullVersion }));

// Documentation page at /docs
app.MapGet("/docs", async (HttpContext context) =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
    if (File.Exists(filePath))
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync(filePath);
        return Results.Empty;
    }
    return Results.NotFound();
});

// Serve static files
app.UseStaticFiles();

// Map MCP endpoints (/sse and /messages)
app.MapMcp();

app.Run();