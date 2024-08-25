using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public InventoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all inventory items for a specific character
    public async Task<IEnumerable<InventoryItem>> GetInventoryItemsForCharacterAsync(int characterId)
    {
        return await _context.InventoryItems
            .Where(item => item.PlayerCharacterId == characterId || item.DMCharacterId == characterId)
            .ToListAsync();
    }

    // Get a specific inventory item by character ID and item ID
    public async Task<InventoryItem> GetInventoryItemByIdAsync(int characterId, int itemId)
    {
        return await _context.InventoryItems
            .FirstOrDefaultAsync(item => (item.PlayerCharacterId == characterId || item.DMCharacterId == characterId) && item.Id == itemId);
    }

    // Get a specific inventory item by item ID only (for moving items)
    public async Task<InventoryItem> GetInventoryItemByIdAsync(int itemId)
    {
        return await _context.InventoryItems
            .FirstOrDefaultAsync(item => item.Id == itemId);
    }

    // Add a new inventory item
    public async Task AddInventoryItemAsync(InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();
    }

    // Update an existing inventory item
    public async Task UpdateInventoryItemAsync(InventoryItem item)
    {
        // Check if the item exists
        var existingItem = await _context.InventoryItems
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == item.Id);

        if (existingItem != null)
        {
            _context.InventoryItems.Update(item);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Cannot update a non-existing item.");
        }
    }

    // Delete an inventory item
    public async Task DeleteInventoryItemAsync(InventoryItem item)
    {
        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    // Get the shared inventory including its items
    public async Task<SharedInventory> GetSharedInventoryAsync()
    {
        return await _context.SharedInventory
            .Include(si => si.InventoryItems)
            .FirstOrDefaultAsync();
    }

    // Update the shared inventory
    public async Task UpdateSharedInventoryAsync(SharedInventory sharedInventory)
    {
        _context.SharedInventory.Update(sharedInventory);
        await _context.SaveChangesAsync();
    }

    // Add an item to the shared inventory
    public async Task AddItemToSharedInventoryAsync(int sharedInventoryId, InventoryItem newItem)
    {
        newItem.SharedInventoryId = sharedInventoryId;
        newItem.PlayerCharacterId = null;
        newItem.DMCharacterId = null;

        _context.InventoryItems.Add(newItem);
        await _context.SaveChangesAsync();
    }

    // Move an item to the shared inventory
    public async Task MoveItemToSharedInventoryAsync(int itemId, int sharedInventoryId)
    {
        var item = await _context.InventoryItems.FindAsync(itemId);

        if (item != null)
        {
            // Clear the character-specific references
            item.PlayerCharacterId = null;
            item.DMCharacterId = null;
            // Set the shared inventory reference
            item.SharedInventoryId = sharedInventoryId;

            _context.InventoryItems.Update(item);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Cannot move a non-existing item.");
        }
    }

    // Remove an item from any inventory
    public async Task RemoveItemFromInventoryAsync(int itemId)
    {
        var item = await _context.InventoryItems.FindAsync(itemId);

        if (item != null)
        {
            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Cannot remove a non-existing item.");
        }
    }
    
}