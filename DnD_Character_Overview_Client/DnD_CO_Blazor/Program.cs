using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DnD_CO_Blazor.Data;

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
builder.Services.AddSingleton<WeatherForecastService>();

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
