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

    [McpServerTool, Description("Create a new dataset")]
    public static async Task<string> CreateDataset(
        SerenityApiClient apiClient,
        [Description("Dataset name")] string name,
        [Description("Dataset description")] string description,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var datasetData = new { name, description };
            var result = await apiClient.CreateDatasetAsync(datasetData, cancellationToken);
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

    [McpServerTool, Description("Append data to an existing table")]
    public static async Task<string> AppendToTable(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        [Description("Data to append as JSON array")] string data,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var appendData = JsonSerializer.Deserialize<object>(data);
            var result = await apiClient.AppendToTableAsync(datasetId, tableId, appendData!, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    [McpServerTool, Description("Replace all data in a table")]
    public static async Task<string> ReplaceTableData(
        SerenityApiClient apiClient,
        [Description("Dataset ID")] string datasetId,
        [Description("Table ID")] string tableId,
        [Description("New data as JSON array")] string data,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var replaceData = JsonSerializer.Deserialize<object>(data);
            var result = await apiClient.ReplaceTableDataAsync(datasetId, tableId, replaceData!, cancellationToken);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
