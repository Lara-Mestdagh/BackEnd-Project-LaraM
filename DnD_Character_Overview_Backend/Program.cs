using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Protos;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on both HTTP (5000) and HTTPS (5001)
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on HTTP for REST APIs on port 5000
    options.ListenLocalhost(5000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1; // HTTP/1.1 only
    });

    // Listen on HTTPS for gRPC and REST APIs on port 5001
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2; // HTTP/1.1 and HTTP/2
        listenOptions.UseHttps(); // Enable HTTPS
    });
});

// Create a Serilog logger and configure it to log to both the console and a file
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console() // Log to console
    .WriteTo.File("Logging/myapp.txt", rollingInterval: RollingInterval.Day) // Log to file with daily rolling
    .CreateLogger();

// Replace the default .NET Core logger with Serilog
builder.Host.UseSerilog(); 

// ********** CONFIGURE SERVICES **********

// 1. Add Entity Framework Core services for MySQL
// Register the DbContext for the application, specifying the MySQL connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// 2. Add Repositories and Services
// Add memory cache services
builder.Services.AddMemoryCache();
// Register application repositories and services for Dependency Injection (DI)
builder.Services.AddScoped<IPlayerCharacterRepository, PlayerCharacterRepository>();
builder.Services.AddScoped<IDMCharacterRepository, DMCharacterRepository>();
builder.Services.AddScoped<IPlayerCharacterService, PlayerCharacterService>();
builder.Services.AddScoped<IDMCharacterService, DMCharacterService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, Services.InventoryService>();

// Register background services
builder.Services.AddHostedService<CharacterStatusMonitoringService>();
builder.Services.AddHostedService<DataCleanupService>();

// Configure services for HotChocolate GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<GraphQL.Query>();

// 3. Add HTTP Client services
// Register HTTP Client for external API integration
builder.Services.AddHttpClient<Open5eApiService>();

// 4. Add AutoMapper for DTO <-> Entity mapping
builder.Services.AddAutoMapper(typeof(CharacterMappingProfile), typeof(InventoryMappingProfile)); 

// 5. Add FluentValidation for Model and DTO Validation
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<PlayerCharacterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DMCharacterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PlayerCharacterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DMCharacterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryItemDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SharedInventoryDtoValidator>();

// 6. Add API Versioning
// Read the API version from appsettings.json
var apiVersion = builder.Configuration["AppSettings:ApiVersion"];

builder.Services.AddApiVersioning(options =>
{
    // Check if the API version is not null or empty and starts with 'v'
    if (!string.IsNullOrEmpty(apiVersion) && apiVersion.StartsWith("v", StringComparison.OrdinalIgnoreCase))
    {
        // Remove theprefix and split by the '.'
        var versionNumber = apiVersion.Substring(1); 
        var versionParts = versionNumber.Split('.'); // Split the version number to handle minor version if exists

        if (int.TryParse(versionParts[0], out var majorVersion)) 
        {
            int minorVersion = 0; // Default minor version

            if (versionParts.Length > 1 && int.TryParse(versionParts[1], out var parsedMinorVersion))
            {
                minorVersion = parsedMinorVersion; // Set parsed minor version
            }

            // Set the default API version using major and minor versions
            options.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
        }
        else
        {
            throw new InvalidOperationException($"Invalid API version format: {apiVersion}");
        }
    }
    else
    {
        throw new InvalidOperationException("API version is not set or not in the expected format 'v#' or 'v#.#'.");
    }

    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// 7. Add Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Version = apiVersion,
        Title = "DnD Character Management API",
        Description = "An API to manage DnD characters and their inventories."
    });

    // Register the custom operation filter
    options.OperationFilter<AddStaticHeaderOperationFilter>();
});

// 8. Add Controllers with JSON Serialization Configuration
// Configure controllers and JSON options
builder.Services.AddControllers(options =>
{
    // Apply the custom route convention to use dynamic versioning
    options.Conventions.Add(new DynamicApiVersionRouteConvention(apiVersion));
})
.AddJsonOptions(options =>
{
    // Configure JSON serialization options
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// 9. Add CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ** 10. Add gRPC Services **
builder.Services.AddGrpc();

// ********** BUILD APPLICATION **********

var app = builder.Build();

// ********** CONFIGURE MIDDLEWARE **********

// HTTPS Redirection Middleware
app.UseHttpsRedirection();

// 1. Exception Handling Middleware
// Global error handling to capture exceptions and return standardized responses
app.UseExceptionHandler("/error");

// 2. API Key Authentication Middleware
app.UseMiddleware<ApiKeyMiddleware>();

// 3. Developer Tools (Swagger) for Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"DnD Character Management API {apiVersion}"));
}

// 4. Routing Middleware
app.UseRouting();

// 5. CORS 
app.UseCors("AllowAllOrigins");

// ********** ENSURE DATABASE IS UP TO DATE **********

// Ensure the database is created and the latest migrations are applied
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// ********** MAP ENDPOINTS **********

// Map all controller endpoints
app.MapControllers();

// Map gRPC endpoints
app.MapGrpcService<CharacterServiceImpl>();
app.MapGrpcService<InventoryServiceImpl>();

// Add a generic route handler for unknown gRPC endpoints
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
});

// GraphQL endpoint
app.MapGraphQL($"/api/{apiVersion}/graphql");

// ********** RUN APPLICATION **********

// Start the web application
app.Run();