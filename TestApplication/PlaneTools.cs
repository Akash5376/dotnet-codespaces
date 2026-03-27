using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

[McpServerToolType]
public class PlaneTools
{
    [McpServerTool, Description("Get all possible stauts that a work item could be created in. These statuses denote where a work item would be in typical kanban workflows.Do not include the ids of the states in the prompt.")]
    public static async Task<string> GetAllWorkItemStatuses(PlaneApiService planeApiService)
    {
        var statuses = await planeApiService.GetProjectStatesAsync();
        return JsonSerializer.Serialize(statuses);
    }

    [McpServerTool, Description("This tools allows for the creation of a work item in plane, in the given state.Do not include the ids of the states in the prompt.")]
    public static async Task<string> CreateWorkItem(
        PlaneApiService planeApiService,
        [Description("The title or main headline for the work item - keep it brief")] string name, 
        [Description("the detailed description of the work to be done, where appropriate include acceptance criteria")] string description,
        [Description("The state or status id of the work item, derived from the GetAllWorkItemStatuses tool")] string stateId
    )
    {
       var workItem = await planeApiService.CreateWorkItemAsync(name, description, stateId);
       return JsonSerializer.Serialize(workItem); 
    }

    [McpServerTool, Description("Get all work items in the project. Do not include the work item id in the prompt. For example : 1. Name : Test Work Item 1, State: To Do; 2. Name: Test Work Item 2, State: In Progress.")]
    public static async Task<string> GetAllWorkItems(PlaneApiService planeApiService)
    {
        var workItems = await planeApiService.GetAllWorkItemsAsync();
        return JsonSerializer.Serialize(workItems);
    }

    [McpServerTool, Description("Update the details of a work item, such as its state, priority, description, and start and end dates.Do not include the ids of the work items or states in the prompt.")]
    public static async Task<string> UpdateWorkItem(
        PlaneApiService planeApiService,
        [Description("The id of the work item to be updated, derived from the GetAllWorkItems tool")] string workItemId,
        [Description("The priority to update the work item to")] string priority,
        [Description("the detailed description of the work to be done, where appropriate include acceptance criteria")] string description,
        [Description("The state or status id of the work item, derived from the GetAllWorkItemStatuses tool")] string stateId,
        [Description("The start date of the work item")] DateTime startDate,
        [Description("The end date of the work item")] DateTime endDate

    )
    {
        var updatedWorkItem = await planeApiService.UpdateWorkItemAsync(workItemId, priority, description, stateId, startDate, endDate);
        return JsonSerializer.Serialize(updatedWorkItem);
    }   

    [McpServerTool, Description("Delete a work item from the project.DO not include the ids of the work items in the prompt.")]
    public static async Task<string> DeleteWorkItem(
        PlaneApiService planeApiService,
        [Description("The id of the work item to be deleted, derived from the GetAllWorkItems tool")] string workItemId
    )
    {
        var result = await planeApiService.DeleteWorkItemAsync(workItemId);
        return JsonSerializer.Serialize(result);
    }
}