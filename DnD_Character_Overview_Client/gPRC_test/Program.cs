using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes; 
using Protos;

var apiKey = "R1RZTUdKTldHWUtCUkZZVVBTUENWSE5CUlZXRFFTTE9NTU1VS05BQUlSV0RJWUJPT1U=";

// Create a call credentials with API key
var callCredentials = CallCredentials.FromInterceptor((context, metadata) =>
{
    metadata.Add("X-API-KEY", apiKey); // Add your API key here with the appropriate header name
    // Remove type from headers if it's only needed in the body
    return Task.CompletedTask;
});

// Create a gRPC channel with HTTPS and API key credentials
var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = new HttpClientHandler
    {
        // This callback allows bypassing SSL certificate validation for local development.
        // This should NOT be used in a production environment.
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    },
    Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials) // Add the API key credentials here
});

var characterClient = new CharacterService.CharacterServiceClient(channel);
var inventoryClient = new InventoryService.InventoryServiceClient(channel);

try
{
    // Call the GetAllCharacters method
    var allCharactersResponse = await characterClient.GetAllCharactersAsync(new EmptyRequest());

    Console.WriteLine("Characters received from server:");
    foreach (var character in allCharactersResponse.Characters)
    {
        Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}");
    }

    // Test inventory methods
    int testCharacterId = 1; // Replace with an actual character ID to test

    // 1. Get all inventory items for a character
    var getInventoryResponse = await inventoryClient.GetInventoryItemsAsync(new GetInventoryItemsRequest 
    { 
        CharacterId = testCharacterId, 
        Type = "player" // Set the type in the request body
    });
    Console.WriteLine($"Inventory items for Character ID {testCharacterId}:");
    foreach (var item in getInventoryResponse.Items)
    {
        Console.WriteLine($"Item ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
    }

    // 2. Get a specific inventory item by ID
    int testItemId = 1; // Replace with an actual item ID to test
    var getItemResponse = await inventoryClient.GetInventoryItemByIdAsync(new GetInventoryItemByIdRequest 
    { 
        CharacterId = testCharacterId, 
        ItemId = testItemId, 
        Type = "player" // Set the type in the request body
    });
    Console.WriteLine($"Inventory item details for Item ID {testItemId}: Name: {getItemResponse.Item.Name}, Quantity: {getItemResponse.Item.Quantity}");

    // 3. Add a new inventory item
    var newItem = new InventoryItem
    {
        Id = 0, // Let server assign an ID
        PlayerCharacterId = testCharacterId,
        DmCharacterId = 0, // Set if applicable
        SharedInventoryId = 0, // Set if applicable
        Name = "New Sword",
        Quantity = 1,
        Weight = 5.0f,
        WeightUnit = "lb"
    };
    var addItemResponse = await inventoryClient.AddInventoryItemAsync(new AddInventoryItemRequest { Item = newItem });
    Console.WriteLine($"Add Inventory Item Response: Success: {addItemResponse.Success}");

    // 4. Update an existing inventory item
    var updateItem = new InventoryItem
    {
        Id = testItemId,
        PlayerCharacterId = testCharacterId,
        DmCharacterId = 0, // Set if applicable
        SharedInventoryId = 0, // Set if applicable
        Name = "Updated Sword",
        Quantity = 2,
        Weight = 5.0f,
        WeightUnit = "lb"
    };
    var updateItemResponse = await inventoryClient.UpdateInventoryItemAsync(new UpdateInventoryItemRequest { Item = updateItem });
    Console.WriteLine($"Update Inventory Item Response: Success: {updateItemResponse.Success}");

    // 5. Delete an inventory item
    var deleteItemResponse = await inventoryClient.DeleteInventoryItemAsync(new DeleteInventoryItemRequest 
    { 
        CharacterId = testCharacterId, 
        ItemId = testItemId, 
        Type = "player" // Set the type in the request body
    });
    Console.WriteLine($"Delete Inventory Item Response: Success: {deleteItemResponse.Success}");
}
catch (RpcException e)
{
    Console.WriteLine($"gRPC Error: {e.Status.StatusCode} - {e.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
