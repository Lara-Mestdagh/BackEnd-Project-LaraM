using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class InventoryItem
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("PlayerCharacter")]
    public int? PlayerCharacterId { get; set; }  // Nullable to allow for DM characters

    [ForeignKey("DMCharacter")]
    public int? DMCharacterId { get; set; }  // Nullable to allow for player characters

    [ForeignKey("SharedInventory")]
    public int? SharedInventoryId { get; set; }  // Nullable to allow for shared inventory

    [Required]
    [StringLength(255)]
    public string? ItemName { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal Weight { get; set; }  // Weight stored in pounds (lb)

    [Required]
    public string WeightUnit { get; set; } = "lb";  // Default unit is pounds

    public PlayerCharacter? PlayerCharacter { get; set; }

    public DMCharacter? DMCharacter { get; set; }

    public SharedInventory? SharedInventory { get; set; }
}
