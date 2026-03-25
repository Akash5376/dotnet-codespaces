using System.Text;
using System.Text.Json;

public class PlaneApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly string _workspace;
    private readonly string _projectId;
    private readonly string _apiKey;
    public PlaneApiService(
        IHttpClientFactory httpClientFactory,
        string baseUrl,
        string workspace,
        string projectId,
        string apiKey
    )
    {
        _httpClientFactory = httpClientFactory;
        _baseUrl = baseUrl.TrimEnd('/');
        _workspace = workspace;
        _projectId = projectId;
        _apiKey = apiKey;
    }

    public async Task<string> GetProjectStatesAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);

            var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/states/";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetProjectStatesAsync: {ex.Message}");       
            return $"Error getting project states: {ex}";    
        }
        
    }

    public async Task<string> CreateWorkItemAsync(string name, string descriptionHtml, string stateId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
        
            var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/work-items/";

            var requestBody = new
            {
                name = name,
                description_html = descriptionHtml,
                state = stateId
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in CreateWorkItemAsync: {ex.Message}");       
            return $"Error creating work item: {ex}";    
        }
 
    }

    public async Task<string> GetAllWorkItemsAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
        
            var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/work-items/";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetAllWorkItemsAsync: {ex.Message}");       
            return $"Error getting all work items: {ex}";    
        }

    }

    public async Task<string> UpdateWorkItemAsync(string workItemId, string? priority, string? descriptionHtml, string? stateId
    ,DateTime? startDate, DateTime? endDate )
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
        
            var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/work-items/{workItemId}";

            var requestBody = new
            {
                priority = priority,
                description_html = descriptionHtml,
                state = stateId,
                start_date = startDate?.ToString("o"),
                end_date = endDate?.ToString("o")
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in UpdateWorkItemAsync: {ex.Message}");       
            return $"Error updating work item: {ex}";    
        }
        
    }

    public async Task<string> DeleteWorkItemAsync(string workItemId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
        
            var url = $"{_baseUrl}/api/v1/workspaces/{_workspace}/projects/{_projectId}/work-items/{workItemId}";

            var response = await httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return $"Work item {workItemId} deleted successfully.";
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in DeleteWorkItemAsync: {ex.Message}");       
            return $"Error deleting work item: {ex}";    
        }

    }
}