using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
    WebRootPath = "wwwroot",
    ApplicationName = "DnD_CO_Blazor"
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents();

// API services
builder.Services.AddScoped<PlayerCharacterService>();
builder.Services.AddScoped<DMCharacterService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<FileService>();


// Add services to the container.
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Register HttpClient with API key
builder.Services.AddHttpClient("AuthorizedClient", (serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    client.DefaultRequestHeaders.Add("X-API-KEY", apiSettings.ApiKey);
});

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5002); // HTTP port
    options.ListenAnyIP(5003, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS port
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
