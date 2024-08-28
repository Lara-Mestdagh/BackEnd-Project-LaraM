using System.ComponentModel.DataAnnotations;

namespace Models;

public class SharedInventory
{
    [Key]
    public int Id { get; set; } = 1;  // Ensuring this is a singleton by always using ID = 1

    public List<InventoryItemDTO> InventoryItems { get; set; } = new List<InventoryItemDTO>();
}
