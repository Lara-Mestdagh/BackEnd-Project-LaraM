using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Configuration;

public class AddStaticHeaderOperationFilter : IOperationFilter
{
    private readonly string _apiKey;

    public AddStaticHeaderOperationFilter(IConfiguration configuration)
    {
        // Read the API key from appsettings.json
        _apiKey = configuration["ApiKeySettings:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "API key is missing in configuration.");
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-API-KEY",
            In = ParameterLocation.Header,
            Required = true, // Set to true if the header is required
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new Microsoft.OpenApi.Any.OpenApiString(_apiKey) // Use the API key from configuration
            },
            Description = "API key needed to access the endpoints"
        });
    }
}
