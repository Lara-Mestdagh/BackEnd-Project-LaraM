namespace DnD_CO_Blazor.Data;

public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;  // Initialize with an empty string to avoid null warning
    public string ApiKey { get; set; } = string.Empty;   // Initialize with an empty string to avoid null warning
}
