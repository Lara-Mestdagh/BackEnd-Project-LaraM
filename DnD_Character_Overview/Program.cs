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


var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IInventoryService, InventoryService>();

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
        // Remove the 'v' prefix and split by the '.' to separate major and minor versions
        var versionNumber = apiVersion.Substring(1); // This will get "4" from "v4" or "4.1" from "v4.1"
        var versionParts = versionNumber.Split('.'); // Split the version number to handle minor version if exists

        if (int.TryParse(versionParts[0], out var majorVersion)) // Parse the major version
        {
            int minorVersion = 0; // Default minor version

            // If there is a minor version, parse it
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

// ********** BUILD APPLICATION **********

var app = builder.Build();

// ********** CONFIGURE MIDDLEWARE **********

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

// GraphQL endpoint
app.MapGraphQL($"/api/{apiVersion}/graphql");

// ********** RUN APPLICATION **********

// Start the web application
app.Run("http://localhost:5000/");