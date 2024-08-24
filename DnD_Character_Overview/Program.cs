using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Firebase Admin SDK configuration
FirebaseApp.Create(new AppOptions()  // Credentials are stored in appsettings.json
{
    Credential = GoogleCredential.FromFile(builder.Configuration["Firebase:CredentialsPath"])
});

// Add Authorization and JWT Authentication
builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/dnd-co"; // Your Firebase project ID
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://securetoken.google.com/dnd-co", // Your Firebase project ID
            ValidAudience = "dnd-co" // Your Firebase project ID
        };
    });

// Add services to the container

// Register HttpClient and Open5eApiService
builder.Services.AddHttpClient<Open5eApiService>();

// Register the repositories
builder.Services.AddScoped<IPlayerCharacterRepository, PlayerCharacterRepository>();
builder.Services.AddScoped<IDMCharacterRepository, DMCharacterRepository>();

// Register the services
builder.Services.AddScoped<IPlayerCharacterService, PlayerCharacterService>();
builder.Services.AddScoped<IDMCharacterService, DMCharacterService>();

// Register FluentValidation
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<PlayerCharacterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DMCharacterValidator>();

// Register the JwtTokenGenerator
builder.Services.AddSingleton<JwtTokenGenerator>();

// Add services for Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);  // Current version of the API
    options.AssumeDefaultVersionWhenUnspecified = true;  // If no version is specified, use the default
    options.ReportApiVersions = true;  // Report API versions supported for the response
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "DnD Character Management API",
        Description = "An API to manage DnD characters and their inventories."
    });
});

// Add controllers and configure JSON serialization to use string enums
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });



// Build the application
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Ensure the database and tables are created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // This will create the database if it doesn't exist and apply migrations if there are any
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline

app.UseExceptionHandler("/error"); // You can set up a generic error handling endpoint

// Use routing
app.UseRouting();

// Map controllers
app.MapControllers();

// Run the application
app.Run("http://localhost:5000/");