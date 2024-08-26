using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Controllers;

[ApiController]
[Route("api/{version:apiVersion}/")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;

    public InventoryController(IInventoryService inventoryService, IMapper mapper)
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
    }

    // Get all inventory items for a specific character
    [HttpGet("characters/{id}/inventory")]
    public async Task<IActionResult> GetInventoryItemsForCharacter(int id)
    {
        var items = await _inventoryService.GetInventoryItemsForCharacterAsync(id);
        var itemDtos = _mapper.Map<IEnumerable<InventoryItemDTO>>(items);
        return Ok(itemDtos);
    }

    // Get a specific inventory item by character ID and item ID
    [HttpGet("characters/{characterId}/inventory/{itemId}")]
    public async Task<IActionResult> GetInventoryItemById(int characterId, int itemId)
    {
        var item = await _inventoryService.GetInventoryItemByIdAsync(characterId, itemId);
        if (item == null)
        {
            return NotFound("Inventory item not found.");
        }

        var itemDto = _mapper.Map<InventoryItemDTO>(item);
        return Ok(itemDto);
    }

    // Add a new inventory item to a character
    [HttpPost("characters/{id}/inventory")]
    public async Task<IActionResult> AddInventoryItem(int id, [FromBody] InventoryItemDTO itemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var item = _mapper.Map<InventoryItem>(itemDto);
        item.PlayerCharacterId = id; // or item.DMCharacterId = id; depending on the character type

        await _inventoryService.AddInventoryItemAsync(item);
        var addedItemDto = _mapper.Map<InventoryItemDTO>(item);
        return CreatedAtAction(nameof(GetInventoryItemById), new { characterId = id, itemId = item.Id }, addedItemDto);
    }

    // Update an existing inventory item
    [HttpPut("characters/{characterId}/inventory/{itemId}")]
    public async Task<IActionResult> UpdateInventoryItem(int characterId, int itemId, [FromBody] InventoryItemDTO itemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingItem = await _inventoryService.GetInventoryItemByIdAsync(characterId, itemId);
        if (existingItem == null)
        {
            return NotFound("Inventory item not found.");
        }

        var updatedItem = _mapper.Map(itemDto, existingItem);
        await _inventoryService.UpdateInventoryItemAsync(updatedItem);
        return NoContent();
    }

    // Delete an inventory item from a character
    [HttpDelete("characters/{characterId}/inventory/{itemId}")]
    public async Task<IActionResult> DeleteInventoryItem(int characterId, int itemId)
    {
        var existingItem = await _inventoryService.GetInventoryItemByIdAsync(characterId, itemId);
        if (existingItem == null)
        {
            return NotFound("Inventory item not found.");
        }

        await _inventoryService.DeleteInventoryItemAsync(characterId, itemId);
        return NoContent();
    }

    // Get shared inventory
    [HttpGet("party/inventory")]
    public async Task<IActionResult> GetSharedInventory()
    {
        var sharedInventory = await _inventoryService.GetSharedInventoryAsync();
        var sharedInventoryDto = _mapper.Map<SharedInventoryDTO>(sharedInventory);
        return Ok(sharedInventoryDto);
    }

    // Update shared inventory
    [HttpPut("party/inventory")]
    public async Task<IActionResult> UpdateSharedInventory([FromBody] SharedInventoryDTO sharedInventoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var sharedInventory = _mapper.Map<SharedInventory>(sharedInventoryDto);
        await _inventoryService.UpdateSharedInventoryAsync(sharedInventory);
        return NoContent();
    }
}