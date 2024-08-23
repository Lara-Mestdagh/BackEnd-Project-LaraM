using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class PlayerCharacter
{
// Constants
    private const int MaxStatValue = 20;

    // Basic Information
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string? Name { get; set; }

    [Required]
    [StringLength(255)]
    public string? Race { get; set; }

    [Required]
    [StringLength(255)]
    public string? PlayerName { get; set; } // IRL player properties

    // Stats
    [Required]
    [Range(1, MaxStatValue)]
    public int Strength { get; set; }

    [Required]
    [Range(1, MaxStatValue)]
    public int Dexterity { get; set; }

    [Required]
    [Range(1, MaxStatValue)]
    public int Constitution { get; set; }

    [Required]
    [Range(1, MaxStatValue)]
    public int Intelligence { get; set; }

    [Required]
    [Range(1, MaxStatValue)]
    public int Wisdom { get; set; }

    [Required]
    [Range(1, MaxStatValue)]
    public int Charisma { get; set; }

    // Hit Points and Armor Class
    [Required]
    public int CurrentHP { get; set; }

    [Required]
    public int MaxHP { get; set; }

    public int TempHP { get; set; } // Temporary HP

    [Required]
    public int ArmorClass { get; set; }

    // Movement
    [Required]
    public int WalkingSpeed { get; set; }

    public int? FlyingSpeed { get; set; }

    public int? SwimmingSpeed { get; set; }

    public int? DarkvisionRange { get; set; }

    // Combat-related
    [Required]
    public int InitiativeModifier => GetModifier(Dexterity); // Based on Dexterity by default

    public List<Conditions>? Conditions { get; set; } // List of current conditions (e.g., poisoned, stunned)

    public List<DamageType> Resistances { get; set; } = new List<DamageType>(); // Damage resistances
    public List<DamageType> Weaknesses { get; set; } = new List<DamageType>(); // Damage weaknesses

    // Status
    [Required]
    public bool IsAlive { get; set; }

    public bool HasOver20Stats { get; set; } = false; // Indicates if stats can exceed 20

    // Languages
    public List<Languages> KnownLanguages { get; set; } = new List<Languages> { Languages.Common }; // Languages known by the character

    // Relationships
    public ICollection<CharacterClass>? CharacterClasses { get; set; } // Character's class(es)
    public ICollection<InventoryItem>? InventoryItems { get; set; } // Inventory items

    // Methods
    public void ValidateStats()
    {
        ValidateStat(Strength, "Strength");
        ValidateStat(Dexterity, "Dexterity");
        ValidateStat(Constitution, "Constitution");
        ValidateStat(Intelligence, "Intelligence");
        ValidateStat(Wisdom, "Wisdom");
        ValidateStat(Charisma, "Charisma");
    }

    private void ValidateStat(int stat, string statName)
    {
        if (stat > MaxStatValue && !HasOver20Stats)
        {
            throw new ValidationException($"{statName} cannot exceed {MaxStatValue} without special abilities.");
        }
    }

    // Method to calculate a stat modifier
    public int GetModifier(int stat)
    {
        return (stat - 10) / 2;
    }

    // Stat modifiers
    public int StrengthModifier => GetModifier(Strength);
    public int DexterityModifier => GetModifier(Dexterity);
    public int ConstitutionModifier => GetModifier(Constitution);
    public int IntelligenceModifier => GetModifier(Intelligence);
    public int WisdomModifier => GetModifier(Wisdom);
    public int CharismaModifier => GetModifier(Charisma);
}