// For details such as what a condition does, the full page of a race and such 
// I will make use of the Open5e API. This class will be responsible for making
// the requests to the API and returning the data.

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Open5e;

public class Open5eApiService
{
    private readonly HttpClient _httpClient;

    public Open5eApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Example method to get condition details
    public async Task<ConditionApiResponse> GetConditionDetailsAsync(string conditionName)
    {
        var response = await _httpClient.GetAsync($"https://api.open5e.com/conditions/{conditionName.ToLower()}/");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ConditionApiResponse>(content);

        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response.");
        }

        return result;
    }

    // Add more methods as needed

    // public async Task<RaceApiResponse> GetRaceDetailsAsync(string raceName)
    // {
    //     return "To be implemented";
    // }

    // public async Task<ClassApiResponse> GetClassDetailsAsync(string className)
    // {
    //     return "To be implemented";
    // }
}