namespace DTOs;

public class SharedInventoryDTO
{
    public int Id { get; set; } = 1; // Singleton by always using ID = 1

    public ICollection<InventoryItemDTO>? InventoryItems { get; set; }
}
