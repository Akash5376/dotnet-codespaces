using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var planeApiKey = builder.Configuration["PlaneAPIKey"];
var baseUrl = builder.Configuration["BaseUrl"];
var workspace = builder.Configuration["Workspace"];
var projectId = builder.Configuration["ProjectId"];

if(string.IsNullOrEmpty(planeApiKey))
    throw new InvalidOperationException("Plane api key is missing.");
if(string.IsNullOrEmpty(baseUrl))
    throw new InvalidOperationException("Plane base url is missing.");
if(string.IsNullOrEmpty(workspace))
    throw new InvalidOperationException("Plane workspace id is missing.");
if(string.IsNullOrEmpty(projectId))
    throw new InvalidOperationException("Plane project id is missing.");

builder.Services.AddHttpClient();
builder.Services.AddSingleton(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new PlaneApiService(httpClientFactory, baseUrl, workspace, projectId, planeApiKey);
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();

