using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private const string API_KEY_HEADER_NAME = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // Check if the request path is for Swagger
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }
        
        // Allow OPTIONS requests to pass through without an API key
        if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // Check if the API key is present in the request headers
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
        {
            _logger.LogWarning("API Key was not provided.");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        // Validate the API key (add your validation logic here)
        var appSettingsApiKey = configuration["ApiKeySettings:ApiKey"];

        // Make sure key is not null or empty
        if (string.IsNullOrEmpty(appSettingsApiKey))
        {
            _logger.LogWarning("API Key is missing in app settings.");
            context.Response.StatusCode = 500; // Internal Server Error
            await context.Response.WriteAsync("API Key is missing in app settings.");
            return;
        }

        if (!appSettingsApiKey.Equals(extractedApiKey))
        {
            _logger.LogWarning("Unauthorized client.");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}