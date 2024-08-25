namespace DTOs;

public class InventoryItemDTO
{
    public int Id { get; set; }

    public int? PlayerCharacterId { get; set; }

    public int? DMCharacterId { get; set; }

    public int? SharedInventoryId { get; set; }


    public string? ItemName { get; set; }

    public int Quantity { get; set; }

    public decimal Weight { get; set; }

    public string WeightUnit { get; set; } = "lb";
}
