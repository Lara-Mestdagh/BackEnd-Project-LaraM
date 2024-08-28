using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

public class IntegrationTests
{
    private readonly HttpClient _client;

    public IntegrationTests()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5000");
        _client.DefaultRequestHeaders.Add("X-API-KEY", "R1RZTUdKTldHWUtCUkZZVVBTUENWSE5CUlZXRFFTTE9NTU1VS05BQUlSV0RJWUJPT1U=");
    }

    // Add type header to inventory requests
    private void AddTypeHeader(HttpClient client, string type)
    {
        client.DefaultRequestHeaders.Remove("type"); // Ensure old header is removed if present
        client.DefaultRequestHeaders.Add("type", type);
    }

    // CHARACTER ENDPOINT TESTS
    [Fact]
    public async Task TestGetDMCharacters()
    {
        var response = await _client.GetAsync("/api/v5/dm-characters");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestGetDMCharacterById()
    {
        // First, retrieve the list to get a valid ID
        var listResponse = await _client.GetAsync("/api/v5/dm-characters");
        listResponse.EnsureSuccessStatusCode();
        var list = JsonConvert.DeserializeObject<dynamic>(await listResponse.Content.ReadAsStringAsync());
        int characterId = list![0].id; // Assuming the list is not empty

        var response = await _client.GetAsync($"/api/v5/dm-characters/{characterId}");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestCreateDMCharacter()
    {
        // Complete payload with all required fields
        var newCharacter = new
        {
            playerName = "DungeonMasterTest",
            name = "Testy the Tester",
            race = "Human",
            strength = 18,
            dexterity = 14,
            constitution = 16,
            intelligence = 8,
            wisdom = 10,
            charisma = 12,
            currentHP = 100,
            maxHP = 100,
            tempHP = 0,
            armorClass = 17,
            walkingSpeed = 30,
            flyingSpeed = (int?)null, // Adjusted to nullable int
            swimmingSpeed = (int?)null, // Adjusted to nullable int
            darkvisionRange = (int?)null, // Adjusted to nullable int
            legendaryActions = "Legendary Slash, Intimidating Roar",
            specialAbilities = "Darkvision, Fury of Blows",
            isAlive = true,
            knownLanguages = new[] { "Common", "Orc" },
            resistances = new[] { "Bludgeoning", "Poison" },
            weaknesses = new[] { "Psychic", "Fire" },
            hasOver20Stats = false
        };

        var content = new StringContent(JsonConvert.SerializeObject(newCharacter), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v5/dm-characters", content);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestUpdateDMCharacter()
    {
        // First, create a character to update
        var createCharacter = new
        {
            playerName = "DungeonMasterTest",
            name = "Testy the Tester",
            race = "Human",
            strength = 18,
            dexterity = 14,
            constitution = 16,
            intelligence = 8,
            wisdom = 10,
            charisma = 12,
            currentHP = 100,
            maxHP = 100,
            tempHP = 0,
            armorClass = 17,
            walkingSpeed = 30,
            flyingSpeed = (int?)null,
            swimmingSpeed = (int?)null,
            darkvisionRange = (int?)null,
            legendaryActions = "Legendary Slash, Intimidating Roar",
            specialAbilities = "Darkvision, Fury of Blows",
            isAlive = true,
            knownLanguages = new[] { "Common", "Orc" },
            resistances = new[] { "Bludgeoning", "Poison" },
            weaknesses = new[] { "Psychic", "Fire" },
            hasOver20Stats = false
        };

        var createResponse = await _client.PostAsync("/api/v5/dm-characters", new StringContent(JsonConvert.SerializeObject(createCharacter), Encoding.UTF8, "application/json"));
        createResponse.EnsureSuccessStatusCode();
        var createdCharacter = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
        int characterId = createdCharacter!.id;

        // Now, update the character
        var updatedCharacter = new
        {
            id = characterId, // Include the ID of the character to update
            playerName = "DungeonMasterTestUpdated",
            name = "Testy the Tester Updated",
            race = "Elf",
            strength = 20,
            dexterity = 16,
            constitution = 18,
            intelligence = 10,
            wisdom = 12,
            charisma = 14,
            currentHP = 110,
            maxHP = 110,
            tempHP = 5,
            armorClass = 19,
            walkingSpeed = 35,
            flyingSpeed = 50,
            swimmingSpeed = (int?)null,
            darkvisionRange = 60,
            legendaryActions = "Legendary Pierce, Fearful Howl",
            specialAbilities = "Darkvision, Rage",
            isAlive = true,
            knownLanguages = new[] { "Common", "Elvish" },
            resistances = new[] { "Bludgeoning", "Cold" },
            weaknesses = new[] { "Psychic", "Radiant" },
            hasOver20Stats = true
        };

        var content = new StringContent(JsonConvert.SerializeObject(updatedCharacter), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/v5/dm-characters/{characterId}", content);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestDeleteDMCharacter()
    {
        // Create a character to delete
        var createResponse = await _client.PostAsync("/api/v5/dm-characters", new StringContent(JsonConvert.SerializeObject(new
        {
            playerName = "DungeonMasterTest",
            name = "Testy the Tester",
            race = "Human",
            strength = 18,
            dexterity = 14,
            constitution = 16,
            intelligence = 8,
            wisdom = 10,
            charisma = 12,
            currentHP = 100,
            maxHP = 100,
            tempHP = 0,
            armorClass = 17,
            walkingSpeed = 30,
            flyingSpeed = (int?)null,
            swimmingSpeed = (int?)null,
            darkvisionRange = (int?)null,
            legendaryActions = "Legendary Slash, Intimidating Roar",
            specialAbilities = "Darkvision, Fury of Blows",
            isAlive = true,
            knownLanguages = new[] { "Common", "Orc" },
            resistances = new[] { "Bludgeoning", "Poison" },
            weaknesses = new[] { "Psychic", "Fire" },
            hasOver20Stats = false
        }), Encoding.UTF8, "application/json"));

        createResponse.EnsureSuccessStatusCode();
        var createdCharacter = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
        int characterId = createdCharacter!.id;

        var response = await _client.DeleteAsync($"/api/v5/dm-characters/{characterId}");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestGetPlayerCharacters()
    {
        var response = await _client.GetAsync("/api/v5/player-characters");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestGetPlayerCharacterById()
    {
        var listResponse = await _client.GetAsync("/api/v5/player-characters");
        listResponse.EnsureSuccessStatusCode();
        var list = JsonConvert.DeserializeObject<dynamic>(await listResponse.Content.ReadAsStringAsync());
        int characterId = list![0].id; // Assuming the list is not empty

        var response = await _client.GetAsync($"/api/v5/player-characters/{characterId}");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestCreatePlayerCharacter()
    {
        var newCharacter = new
        {
            playerName = "PlayerTest",
            name = "Legolas",
            race = "Elf",
            strength = 15,
            dexterity = 20,
            constitution = 12,
            intelligence = 14,
            wisdom = 13,
            charisma = 18,
            currentHP = 50,
            maxHP = 50,
            tempHP = 5,
            armorClass = 15,
            walkingSpeed = 30,
            flyingSpeed = (int?)null,
            swimmingSpeed = (int?)null,
            darkvisionRange = (int?)null,
            isAlive = true,
            knownLanguages = new[] { "Common", "Elvish" },
            resistances = new[] { "Fire" },
            weaknesses = new[] { "Cold" },
            hasOver20Stats = false
        };

        var content = new StringContent(JsonConvert.SerializeObject(newCharacter), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v5/player-characters", content);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestUpdatePlayerCharacter()
    {
        // First, create a character to update
        var newCharacter = new
        {
            playerName = "PlayerTest",
            name = "Legolas",
            race = "Elf",
            strength = 15,
            dexterity = 20,
            constitution = 12,
            intelligence = 14,
            wisdom = 13,
            charisma = 18,
            currentHP = 50,
            maxHP = 50,
            tempHP = 5,
            armorClass = 15,
            walkingSpeed = 30,
            flyingSpeed = (int?)null,
            swimmingSpeed = (int?)null,
            darkvisionRange = (int?)null,
            isAlive = true,
            knownLanguages = new[] { "Common", "Elvish" },
            resistances = new[] { "Fire" },
            weaknesses = new[] { "Cold" },
            hasOver20Stats = false
        };

        var createResponse = await _client.PostAsync("/api/v5/player-characters", new StringContent(JsonConvert.SerializeObject(newCharacter), Encoding.UTF8, "application/json"));
        createResponse.EnsureSuccessStatusCode();
        var createdCharacter = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
        int characterId = createdCharacter!.id;

        var updatedCharacter = new
        {
            id = characterId,
            playerName = "PlayerTest",
            name = "Aragorn",
            race = "Human",
            strength = 16,
            dexterity = 18,
            constitution = 14,
            intelligence = 12,
            wisdom = 15,
            charisma = 16,
            currentHP = 60,
            maxHP = 60,
            tempHP = 0,
            armorClass = 18,
            walkingSpeed = 30,
            flyingSpeed = (int?)null,
            swimmingSpeed = (int?)null,
            darkvisionRange = (int?)null,
            isAlive = true,
            knownLanguages = new[] { "Common", "Dwarvish" },
            resistances = new[] { "Lightning" },
            weaknesses = new[] { "Poison" },
            hasOver20Stats = false
        };

        var content = new StringContent(JsonConvert.SerializeObject(updatedCharacter), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/v5/player-characters/{characterId}", content);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestDeletePlayerCharacter()
    {
        // First, create a character to delete
        var newCharacter = new
        {
            playerName = "PlayerTest",
            name = "Legolas",
            race = "Elf",
            strength = 15,
            dexterity = 20,
            constitution = 12,
            intelligence = 14,
            wisdom = 13,
            charisma = 18,
            currentHP = 50,
            maxHP = 50,
            tempHP = 5,
            armorClass = 15,
            walkingSpeed = 30,
            flyingSpeed = (int?)null,
            swimmingSpeed = (int?)null,
            darkvisionRange = (int?)null,
            isAlive = true,
            knownLanguages = new[] { "Common", "Elvish" },
            resistances = new[] { "Fire" },
            weaknesses = new[] { "Cold" },
            hasOver20Stats = false
        };

        var createResponse = await _client.PostAsync("/api/v5/player-characters", new StringContent(JsonConvert.SerializeObject(newCharacter), Encoding.UTF8, "application/json"));
        createResponse.EnsureSuccessStatusCode();
        var createdCharacter = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
        int characterId = createdCharacter!.id;

        var response = await _client.DeleteAsync($"/api/v5/player-characters/{characterId}");
        response.EnsureSuccessStatusCode();
    }

    // INVENTORY ENDPOINT TESTS
    [Fact]
    public async Task TestGetInventoryForCharacter()
    {
        AddTypeHeader(_client, "player"); // Add type header
        int characterId = 1; // Example ID
        var response = await _client.GetAsync($"/api/v5/characters/{characterId}/inventory");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestAddInventoryItem()
    {
        AddTypeHeader(_client, "player"); // Add type header
        int characterId = 1; // Example ID
        var newItem = new { ItemName = "Health Potion", Quantity = 1, Weight = 0.5 };
        var content = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/api/v5/characters/{characterId}/inventory", content);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestUpdateInventoryItem()
    {
        AddTypeHeader(_client, "player"); // Add type header
        int characterId = 1; // Example character ID
        int sharedInventoryId = 1; // Example shared inventory ID

        // First, create a new item to ensure there is something to update
        var newItem = new
        {
            itemName = "Sword",
            quantity = 1,
            weight = 5.0,
            weightUnit = "lb",
            playerCharacterId = characterId,
            dmCharacterId = (int?)null,
            sharedInventoryId = sharedInventoryId
        };

        var createContent = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync($"/api/v5/characters/{characterId}/inventory", createContent);
        createResponse.EnsureSuccessStatusCode();
        var createdItem = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
        int createdItemId = createdItem!.id;

        // Prepare the updated item payload
        var updatedItem = new
        {
            id = createdItemId,
            itemName = "Updated Sword", // New item name for update
            quantity = 2,  // Updated quantity
            weight = 5.5,
            weightUnit = "lb",
            playerCharacterId = characterId,
            dmCharacterId = (int?)null, 
            sharedInventoryId = sharedInventoryId
        };

        var updateContent = new StringContent(JsonConvert.SerializeObject(updatedItem), Encoding.UTF8, "application/json");

        // Perform the PUT request to update the item
        var response = await _client.PutAsync($"/api/v5/characters/{characterId}/inventory/{createdItemId}", updateContent);

        // Check for the expected 204 No Content status code
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteInventoryItem()
    {
        AddTypeHeader(_client, "player"); // Add type header
        int characterId = 1; // Example character ID
        int sharedInventoryId = 1; // Example shared inventory ID

        // First, create a new item to ensure there is something to delete
        var newItem = new
        {
            itemName = "Health Potion",
            quantity = 1,
            weight = 0.5,
            weightUnit = "lb",
            playerCharacterId = characterId,
            dmCharacterId = (int?)null,
            sharedInventoryId = sharedInventoryId
        };

        var createContent = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync($"/api/v5/characters/{characterId}/inventory", createContent);
        createResponse.EnsureSuccessStatusCode();
        var createdItem = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
        int createdItemId = createdItem!.id;

        // Now delete the item
        var response = await _client.DeleteAsync($"/api/v5/characters/{characterId}/inventory/{createdItemId}");

        // Check for the expected 204 No Content status code
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task TestGetPartyInventory()
    {
        var response = await _client.GetAsync("/api/v5/party/inventory");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody); // Ensure that the inventory is not empty
    }

    // FILE MANAGEMENT ENDPOINT TESTS
    [Fact]
    public async Task TestFileUpload()
    {
        // Create a new multipart form data content to simulate file upload
        var formData = new MultipartFormDataContent();

        // Make sure the path is correct and the file exists
        string filePath = @"D:/Documents/MCT/Semester_4/Backend_Dev/Project/BD-Project-LaraM/DnD_Character_Overview_Tests/goblin.png"; // Change this to the actual path of your file
        
        // Read the actual image file into a byte array
        byte[] fileBytes = File.ReadAllBytes(filePath);

        // Create ByteArrayContent from the file byte array
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png"); // Ensure correct content type

        // Add the file content to the form data with proper metadata
        formData.Add(fileContent, "file", "goblin.png");

        // Add characterId and characterType as required by the API
        formData.Add(new StringContent("2"), "characterId"); // Example character ID
        formData.Add(new StringContent("dm"), "type"); // Example character type

        // Send the POST request to upload the file
        var response = await _client.PostAsync("/api/v5/files/upload", formData);

        // Debugging output to check the server's response if the test fails
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Upload failed with status code {response.StatusCode}: {errorResponse}");
        }

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        // Read and validate the response body
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task TestDownloadInventory()
    {
        AddTypeHeader(_client, "player"); // Add type header
        int characterId = 1; // Example ID
        var format = "csv"; // or "pdf"
        var response = await _client.GetAsync($"/api/v5/files/download/inventory/{characterId}/{format}");
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType); // Adjusted to match "text/csv" if that's the actual content type
    }

    [Fact]
    public async Task TestDownloadPartyInventory()
    {
        var format = "csv"; // or "pdf"
        var response = await _client.GetAsync($"/api/v5/files/download/party-inventory/{format}");
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType); // Adjusted to match "text/csv" if that's the actual content type
    }

    [Fact]
    public async Task TestDownloadCharacterSheet()
    {
        int characterId = 1; // Example ID
        var format = "csv"; // or "pdf"
        var response = await _client.GetAsync($"/api/v5/files/download/character-sheet/{characterId}/{format}");
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType); // Adjusted to match "text/csv" if that's the actual content type
    }
}
