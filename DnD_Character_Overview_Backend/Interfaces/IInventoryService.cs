namespace Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<InventoryItem>> GetInventoryItemsForCharacterAsync(int characterId, string type);
    Task<InventoryItem> GetInventoryItemByIdAsync(int characterId, int itemId, string type);
    Task AddInventoryItemAsync(InventoryItem item);
    Task UpdateInventoryItemAsync(InventoryItem item);
    Task<bool> DeleteInventoryItemAsync(int characterId, int itemId, string type);
    Task<SharedInventory> GetSharedInventoryAsync();
    Task UpdateSharedInventoryAsync(SharedInventory sharedInventory);
    Task AddItemToSharedInventoryAsync(int sharedInventoryId, InventoryItem newItem);
    Task MoveItemToSharedInventoryAsync(int itemId, int sharedInventoryId);
    Task RemoveItemFromInventoryAsync(int itemId);
}
