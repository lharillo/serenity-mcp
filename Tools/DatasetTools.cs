using ModelContextProtocol.Server;
using SerenityStarMcp.Services;
using System.ComponentModel;
using System.Text.Json;

namespace SerenityStarMcp.Tools;

/// <summary>
/// MCP tools for managing Serenity Star datasets and tables
/// </summary>
[McpServerToolType]
public class DatasetTools
{
    // ================================================================================
    // DATASET OPERATIONS
    // ================================================================================

    [McpServerTool, Description("List all datasets with pagination")]
    public static async Task<string> ListDatasets(
        SerenityApiClient apiClient,
        [Description("Page number (default: 1)")] int page = 1,
        [Description("Page size (default: 20)")] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.ListDatasetsAsync(page, pageSize, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Create a new dataset with an initial table. Requires CSV data content for the dataset file.")]
    public static async Task<string> CreateDataset(
        SerenityApiClient apiClient,
        [Description("Dataset unique identifier/code (alphanumeric with hyphens, max 64 chars, e.g. 'sales-data')")] string identifier,
        [Description("Initial table unique identifier/code (alphanumeric with hyphens, max 64 chars, e.g. 'transactions')")] string tableIdentifier,
        [Description("CSV file content as string. First row must be headers. Example: 'id,name,value\\n1,test,100\\n2,test2,200'")] string csvContent,
        [Description("Dataset display name (max 64 chars, e.g. 'Sales Data')")] string? displayName = null,
        [Description("Dataset description (max 4000 chars)")] string? description = null,
        [Description("Initial table display name (max 64 chars, e.g. 'Transactions')")] string? tableDisplayName = null,
        [Description("Initial table description (max 4000 chars)")] string? tableDescription = null,
        [Description("CSV file name (default: 'data.csv')")] string? fileName = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            var result = await apiClient.CreateDatasetAsync(
                identifier: identifier,
                tableIdentifier: tableIdentifier,
                displayName: displayName,
                description: description,
                tableDisplayName: tableDisplayName,
                tableDescription: tableDescription,
                fileContent: fileBytes,
                fileName: fileName ?? "data.csv",
                cancellationToken: cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Get a dataset by ID")]
    public static async Task<string> GetDataset(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.GetDatasetAsync(datasetId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update a dataset")]
    public static async Task<string> UpdateDataset(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Updated name (optional)")] string? name = null,
        [Description("Updated description (optional)")] string? description = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var updateData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(name)) updateData["name"] = name;
            if (!string.IsNullOrEmpty(description)) updateData["description"] = description;

            var result = await apiClient.UpdateDatasetAsync(datasetId, updateData, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Delete a dataset")]
    public static async Task<string> DeleteDataset(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.DeleteDatasetAsync(datasetId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Query a dataset with natural language or SQL")]
    public static async Task<string> QueryDataset(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Query string (natural language or SQL)")] string query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryData = new { query };
            var result = await apiClient.QueryDatasetAsync(datasetId, queryData, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    // ================================================================================
    // TABLE OPERATIONS
    // ================================================================================

    [McpServerTool, Description("Create a new table in a dataset")]
    public static async Task<string> CreateTable(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table name")] string tableName,
        [Description("Table schema as JSON string")] string schema,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tableData = new { name = tableName, schema = JsonSerializer.Deserialize<object>(schema) };
            var result = await apiClient.CreateTableAsync(datasetId, tableData, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Update a table in a dataset")]
    public static async Task<string> UpdateTable(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        [Description("Updated table name (optional)")] string? tableName = null,
        [Description("Updated schema as JSON string (optional)")] string? schema = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var updateData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(tableName)) updateData["name"] = tableName;
            if (!string.IsNullOrEmpty(schema)) updateData["schema"] = JsonSerializer.Deserialize<object>(schema);

            var result = await apiClient.UpdateTableAsync(datasetId, tableId, updateData, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Delete a table from a dataset")]
    public static async Task<string> DeleteTable(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await apiClient.DeleteTableAsync(datasetId, tableId, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Append data to an existing table. Requires CSV file content with headers matching the table schema.")]
    public static async Task<string> AppendToTable(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        [Description("CSV file content as string. First row must be headers matching the table schema. Example: 'col1,col2\nval1,val2'")] string csvContent,
        [Description("CSV file name (default: 'data.csv')")] string? fileName = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            var result = await apiClient.AppendToTableAsync(datasetId, tableId, fileBytes, fileName ?? "data.csv", cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Replace all data in a table. Requires CSV file content with headers matching the table schema.")]
    public static async Task<string> ReplaceTableData(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        [Description("CSV file content as string. First row must be headers matching the table schema. Example: 'col1,col2\nval1,val2'")] string csvContent,
        [Description("CSV file name (default: 'data.csv')")] string? fileName = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            var result = await apiClient.ReplaceTableDataAsync(datasetId, tableId, fileBytes, fileName ?? "data.csv", cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
