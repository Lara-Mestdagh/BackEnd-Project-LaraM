using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Register HttpClient and DndApiService
builder.Services.AddHttpClient<Open5eApiService>();

// Add services for Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Ensure the database and tables are created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // This will create the database if it doesn't exist and apply migrations if there are any
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
app.MapGet("/", () => "Hello World!");

app.Run("http://localhost:5000/");