using Microsoft.AspNetCore.Mvc.ApplicationModels;

public class DynamicApiVersionRouteConvention : IApplicationModelConvention
{
    private readonly string _apiVersion;

    public DynamicApiVersionRouteConvention(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public void Apply(ApplicationModel application)
    {
        // Iterate over all controllers in the application
        foreach (var controller in application.Controllers)
        {
            // Get the first selector's AttributeRouteModel (if it exists)
            var routeAttribute = controller.Selectors.FirstOrDefault()?.AttributeRouteModel;

            if (routeAttribute != null)
            {
                // Replace the "{version:apiVersion}" placeholder with the actual version
                routeAttribute.Template = routeAttribute.Template!.Replace("{version:apiVersion}", _apiVersion);
            }
        }
    }
}
