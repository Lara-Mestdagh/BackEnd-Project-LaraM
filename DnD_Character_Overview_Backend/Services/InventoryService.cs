using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    // Retrieve all inventory items for a specific character based on type
    public async Task<IEnumerable<InventoryItem>> GetInventoryItemsForCharacterAsync(int characterId, string type)
    {
        return await _repository.GetInventoryItemsForCharacterAsync(characterId, type);
    }

    // Retrieve a specific inventory item by its ID and type
    public async Task<InventoryItem> GetInventoryItemByIdAsync(int characterId, int itemId, string type)
    {
        return await _repository.GetInventoryItemByIdAsync(characterId, itemId, type);
    }

    // Add a new inventory item to a character or shared inventory
    public async Task AddInventoryItemAsync(InventoryItem item)
    {
        await _repository.AddInventoryItemAsync(item);
    }

    // Update an existing inventory item
    public async Task UpdateInventoryItemAsync(InventoryItem item)
    {
        await _repository.UpdateInventoryItemAsync(item);
    }

    // Delete an inventory item from a character or shared inventory based on type
    public async Task<bool> DeleteInventoryItemAsync(int characterId, int itemId, string type)
    {
        var item = await _repository.GetInventoryItemByIdAsync(characterId, itemId, type);
        
        if (item != null)
        {
            await _repository.DeleteInventoryItemAsync(item);
            return true;  
        }
        
        return false; 
    }

    // Retrieve the shared inventory
    public async Task<SharedInventory> GetSharedInventoryAsync()
    {
        return await _repository.GetSharedInventoryAsync();
    }

    // Update the shared inventory
    public async Task UpdateSharedInventoryAsync(SharedInventory sharedInventory)
    {
        await _repository.UpdateSharedInventoryAsync(sharedInventory);
    }

    // Add item to shared inventory
    public async Task AddItemToSharedInventoryAsync(int sharedInventoryId, InventoryItem newItem)
    {
        // Set the relationship to the SharedInventory
        newItem.SharedInventoryId = sharedInventoryId;
        newItem.PlayerCharacterId = null;
        newItem.DMCharacterId = null;

        // Add the new item
        await _repository.AddInventoryItemAsync(newItem);
    }

    // Move item between inventories (e.g., from a character's inventory to the shared inventory)
    public async Task MoveItemToSharedInventoryAsync(int itemId, int sharedInventoryId)
    {
        var item = await _repository.GetInventoryItemByIdAsync(itemId);
        
        if (item == null)
        {
            // Handle item not found
            throw new InvalidOperationException("Item not found");
        }

        // Detach item from current parent
        item.PlayerCharacterId = null;
        item.DMCharacterId = null;

        // Attach item to shared inventory
        item.SharedInventoryId = sharedInventoryId;

        // Update item in the database
        await _repository.UpdateInventoryItemAsync(item);
    }

    // Remove item from any inventory
    public async Task RemoveItemFromInventoryAsync(int itemId)
    {
        var item = await _repository.GetInventoryItemByIdAsync(itemId);

        if (item == null)
        {
            // Handle item not found
            throw new InvalidOperationException("Item not found");
        }

        await _repository.DeleteInventoryItemAsync(item);
    }
}
