using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace Services;

public class FileService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public FileService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task DownloadCharacterSheetAsync(string endpoint)
    {
        // Make the request to download the file
        var response = await _httpClient.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsByteArrayAsync();
            var fileName = "download.csv"; // Customize this if needed

            // Use JS Interop to save the file
            await _jsRuntime.InvokeVoidAsync("saveAsFile", fileName, Convert.ToBase64String(content));
        }
        else
        {
            // Handle error
            throw new Exception($"Failed to download file: {response.ReasonPhrase}");
        }
    }
}