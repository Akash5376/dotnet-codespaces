using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

[McpServerToolType]
public class PlaneTools
{
    [McpServerTool, Description("Get all possible stauts that a work item could be created in.")]
    public static async Task<string> GetAllWorkItemStatuses(PlaneApiService planeApiService)
    {
        var statuses = await planeApiService.GetProjectStatesAsync();
        return JsonSerializer.Serialize(statuses);
    }

    [McpServerTool, Description("This tools allows for the creation of a work item.")]
    public static async Task<string> CreateWorkItem(
        PlaneApiService planeApiService,
        [Description("The title or main headline for the work item")] string name, 
        [Description("the detalied desc of the work")] string description,
        [Description("The state or status id of the work item")] string stateid
    )
    {
       var workItem = await planeApiService.CreateWorkItemAsync(name, description, stateid);
       return JsonSerializer.Serialize(workItem); 
    }
}