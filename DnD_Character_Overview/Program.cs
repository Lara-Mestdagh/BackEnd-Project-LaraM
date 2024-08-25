using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;


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

// 1. Firebase Admin SDK Configuration
// Current firebase was not working, deleted for now, will add back later

// 2. Add Entity Framework Core services for MySQL
// Register the DbContext for the application, specifying the MySQL connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// 3. Add Repositories and Services
// Register application repositories and services for Dependency Injection (DI)
builder.Services.AddScoped<IPlayerCharacterRepository, PlayerCharacterRepository>();
builder.Services.AddScoped<IDMCharacterRepository, DMCharacterRepository>();
builder.Services.AddScoped<IPlayerCharacterService, PlayerCharacterService>();
builder.Services.AddScoped<IDMCharacterService, DMCharacterService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

// 4. Add HTTP Client services
// Register HTTP Client for external API integration
builder.Services.AddHttpClient<Open5eApiService>();

// 5. Add AutoMapper for DTO <-> Entity mapping
builder.Services.AddAutoMapper(typeof(CharacterMappingProfile), typeof(InventoryMappingProfile)); 

// 6. Add FluentValidation for Model and DTO Validation
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<PlayerCharacterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DMCharacterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PlayerCharacterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DMCharacterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryItemDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SharedInventoryDtoValidator>();

// 7. Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);  // Set default API version
    options.AssumeDefaultVersionWhenUnspecified = true;  // Use default version if none is specified
    options.ReportApiVersions = true;  // Include supported versions in response headers
});

// 8. Add Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v2",
        Title = "DnD Character Management API",
        Description = "An API to manage DnD characters and their inventories."
    });
});

// 9. Add Controllers with JSON Serialization Configuration
// Configure controllers and JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ********** BUILD APPLICATION **********

var app = builder.Build();

// ********** CONFIGURE MIDDLEWARE **********

// 1. Exception Handling Middleware
app.UseExceptionHandler("/error");

// 2. Developer Tools (Swagger) for Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. Routing Middleware
app.UseRouting();

// 4. Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// 5. CORS (Optional)
// Uncomment and configure if your frontend is hosted on a different domain
// app.UseCors(policy => policy
//     .AllowAnyOrigin()
//     .AllowAnyMethod()

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

// ********** RUN APPLICATION **********

// Start the web application
app.Run("http://localhost:5000/");