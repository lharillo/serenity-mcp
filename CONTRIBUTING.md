# Contributing to Serenity Star MCP Server

Thank you for your interest in contributing to the Serenity Star MCP Server!

## Development Setup

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional, for containerization)
- A Serenity Star API key (get one at [serenitystar.ai](https://serenitystar.ai))

### Local Development

1. Clone the repository:
```bash
git clone https://github.com/subgenai/serenity-mcp.git
cd serenity-mcp
```

2. Build the project:
```bash
dotnet build
```

3. Run the server:
```bash
dotnet run
```

The server will start on `http://localhost:8080`

### Testing

Configure your MCP client to connect to the local server:

```json
{
  "mcpServers": {
    "serenity-star-dev": {
      "url": "http://localhost:8080/sse",
      "headers": {
        "X-Serenity-API-Key": "YOUR_API_KEY"
      }
    }
  }
}
```

## Code Style

- Follow standard C# conventions
- Use meaningful variable and method names
- Add XML documentation comments to public APIs
- Keep methods focused and concise

## Adding New Tools

To add a new MCP tool:

1. Create or update a file in `/Tools/`
2. Add the `[McpServerToolType]` attribute to the class
3. Add `[McpServerTool]` and `[Description]` attributes to methods
4. Use `SerenityApiClient` for API calls
5. Return JSON-serialized results

Example:

```csharp
[McpServerToolType]
public class MyTools
{
    [McpServerTool, Description("My new tool description")]
    public static async Task<string> MyTool(
        SerenityApiClient apiClient,
        [Description("Parameter description")] string param,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.SomeMethodAsync(param, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
```

## Pull Request Process

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Test thoroughly
5. Commit with clear messages (`git commit -m 'Add amazing feature'`)
6. Push to your fork (`git push origin feature/amazing-feature`)
7. Open a Pull Request

### Pull Request Guidelines

- Provide a clear description of the changes
- Reference any related issues
- Include tests if applicable
- Update documentation as needed
- Ensure all checks pass

## Reporting Issues

When reporting issues, please include:

- A clear description of the problem
- Steps to reproduce
- Expected vs actual behavior
- Your environment (OS, .NET version, etc.)
- Relevant logs or error messages

## Documentation

- Keep README.md up to date
- Update CHANGELOG.md for notable changes
- Add/update XML comments for public APIs
- Update landing page (`wwwroot/index.html`) if needed

## Code of Conduct

- Be respectful and inclusive
- Provide constructive feedback
- Focus on the code, not the person
- Help maintain a positive community

## Questions?

Feel free to open an issue for questions or reach out to the maintainers.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing! 🙏
