using System.ComponentModel.DataAnnotations;
namespace Models;

public class DMCharacter
{
    // DM character properties
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string? Name { get; set; }

    [Required]
    [StringLength(255)]
    public string? Race { get; set; }

    public string? Description { get; set; }

    public int? CurrentHP { get; set; }

    [Required]
    public int MaxHP { get; set; }

    public int TempHP { get; set; }

    public string? Conditions { get; set; }

    [Required]
    public int WalkingSpeed { get; set; }

    public int? FlyingSpeed { get; set; }

    public int? SwimmingSpeed { get; set; }

    public int? DarkvisionRange { get; set; }

    // Special abilities and actions 
    public string? LegendaryActions { get; set; }

    public string? SpecialAbilities { get; set; }

    // Character class
    public ICollection<CharacterClass>? CharacterClasses { get; set; }
    // Inventory items
    public ICollection<InventoryItem>? InventoryItems { get; set; }
}