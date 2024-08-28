using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services;

public class DMCharacterService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DMCharacterService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Methods for DM Characters
    public async Task<List<DMCharacter>> GetDMCharactersAsync()
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.GetFromJsonAsync<List<DMCharacter>>("dm-characters");

        if (response == null)
        {
            Console.WriteLine("No DM characters found.");
            return new List<DMCharacter>();
        }

        return response;
    }

    public async Task<DMCharacter> GetDMCharacterByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        return await client.GetFromJsonAsync<DMCharacter>($"dm-characters/{id}");
    }

    public async Task AddDMCharacterAsync(DMCharacter character)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.PostAsJsonAsync("dm-characters", character);

        if (!response.IsSuccessStatusCode)
        {
            // Log the error or throw an exception
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
            throw new Exception($"Failed to add character: {response.ReasonPhrase}");
        }
    }

    public async Task UpdateDMCharacterAsync(DMCharacter character)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");

        // Log character data
        Console.WriteLine($"Updating character with ID {character.Id}: {System.Text.Json.JsonSerializer.Serialize(character)}");

        // Update the dn character, including character classes
        var characterResponse = await client.PutAsJsonAsync($"dm-characters/{character.Id}", character);

        if (!characterResponse.IsSuccessStatusCode)
        {
            // Log the error or throw an exception
            var errorContent = await characterResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {characterResponse.StatusCode} - {errorContent}");
            throw new Exception($"Failed to update character: {characterResponse.ReasonPhrase}");
        }
    }

    public async Task DeleteDMCharacterAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        await client.DeleteAsync($"dm-characters/{id}");
    }
}