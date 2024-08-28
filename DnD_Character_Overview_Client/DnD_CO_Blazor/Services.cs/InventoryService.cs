using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;


namespace Services
{
    public class InventoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InventoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<InventoryItem>> GetPartyInventoryAsync()
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            try
            {
                // Fetch the raw JSON response
                var response = await client.GetAsync("party/inventory");

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON response as a dynamic object
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonDocument.Parse(jsonResponse).RootElement;

                    // Check if the "inventoryItems" element exists in the JSON response
                    if (jsonObject.TryGetProperty("inventoryItems", out JsonElement inventoryItemsElement))
                    {
                        // Deserialize the "inventoryItems" array to a list of InventoryItemDTO
                        var inventoryItemsDto = JsonSerializer.Deserialize<List<InventoryItemDTO>>(inventoryItemsElement.GetRawText(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        // Convert DTOs to InventoryItem models
                        return inventoryItemsDto?.Select(dto => new InventoryItem
                        {
                            Id = dto.Id,
                            PlayerCharacterId = dto.PlayerCharacterId,
                            DMCharacterId = dto.DMCharacterId,
                            SharedInventoryId = dto.SharedInventoryId,
                            ItemName = dto.ItemName,
                            Quantity = dto.Quantity,
                            Weight = dto.Weight,
                            WeightUnit = dto.WeightUnit
                        }).ToList() ?? new List<InventoryItem>();
                    }
                    else
                    {
                        Console.WriteLine("No inventory items found in the JSON response.");
                        return new List<InventoryItem>();
                    }
                }
                else
                {
                    Console.WriteLine($"Error fetching party inventory: {response.ReasonPhrase}");
                    return new List<InventoryItem>();
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON error fetching party inventory: {ex.Message}");
                return new List<InventoryItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching party inventory: {ex.Message}");
                return new List<InventoryItem>();
            }
        }

        public async Task<List<PlayerCharacter>> GetPlayerCharactersAsync()
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            try
            {
                var response = await client.GetFromJsonAsync<List<PlayerCharacter>>("player-characters");
                Console.WriteLine($"Fetched {response?.Count} player characters.");
                return response ?? new List<PlayerCharacter>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching player characters: {ex.Message}");
                return new List<PlayerCharacter>();
            }
        }

        public async Task<List<DMCharacter>> GetDMCharactersAsync()
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            try
            {
                var response = await client.GetFromJsonAsync<List<DMCharacter>>("dm-characters");
                return response ?? new List<DMCharacter>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching DM characters: {ex.Message}");
                return new List<DMCharacter>();
            }
        }

        public async Task<List<InventoryItem>> GetInventoryItemsForCharacterAsync(int characterId, string type)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");

            try
            {
                client.DefaultRequestHeaders.Remove("type");
                client.DefaultRequestHeaders.Add("type", type); // Set the type in the request header

                var response = await client.GetAsync($"characters/{characterId}/inventory");
                if (response.IsSuccessStatusCode)
                {
                    var inventoryItems = await response.Content.ReadFromJsonAsync<List<InventoryItem>>();
                    return inventoryItems ?? new List<InventoryItem>();
                }
                else
                {
                    Console.WriteLine($"Error fetching inventory for character {characterId}: {response.ReasonPhrase}");
                    return new List<InventoryItem>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching inventory for character {characterId}: {ex.Message}");
                return new List<InventoryItem>();
            }
        }

        public async Task AddInventoryItemAsync(int characterId, InventoryItem newItem)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            try
            {
                var response = await client.PostAsJsonAsync($"characters/{characterId}/inventory", newItem);
                response.EnsureSuccessStatusCode(); // Throws if the status code is not successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to inventory for ID {characterId}: {ex.Message}");
            }
        }

        public async Task DeleteInventoryItemAsync(int characterId, int itemId)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            try
            {
                var response = await client.DeleteAsync($"characters/{characterId}/inventory/{itemId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item {itemId} from inventory for ID {characterId}: {ex.Message}");
            }
        }


        public async Task PopulateCharacterInventories(List<PlayerCharacter> playerCharacters, List<DMCharacter> dmCharacters)
        {
            foreach (var player in playerCharacters)
            {
                player.InventoryItems = await GetInventoryItemsForCharacterAsync(player.Id, "player");
                Console.WriteLine($"Assigned {player.InventoryItems.Count} items to player {player.Name}");
            }

            foreach (var dm in dmCharacters)
            {
                dm.InventoryItems = await GetInventoryItemsForCharacterAsync(dm.Id, "dm");
                Console.WriteLine($"Assigned {dm.InventoryItems.Count} items to DM character {dm.Name}");
            }
        }
    }
}
