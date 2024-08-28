// Since both the player and DM characters share the same properties, we can create a base class that both inherit from.

namespace Models;

public abstract class CharacterBase
{
    // Constants
    protected const int MaxStatValue = 20;

    // Basic Information
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string? Name { get; set; }

    [Required]
    [StringLength(255)]
    public string? Race { get; set; }

    [StringLength(255)]
    public string? ImagePath { get; set; } // Path to the character's image

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
    public int MaxHP { get; set; }

    public int? CurrentHP { get; set; }

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

    public List<DamageType>? Resistances { get; set; } = new List<DamageType>(); // Damage resistances
    public List<DamageType>? Weaknesses { get; set; } = new List<DamageType>(); // Damage weaknesses

    public List<Conditions>? Conditions { get; set; } = new List<Conditions>(); // List of current conditions (e.g., poisoned, stunned)

    // Status
    [Required]
    public bool IsAlive { get; set; }

    public bool HasOver20Stats { get; set; } = false; // Indicates if stats can exceed 20

    // Languages
    public List<Languages> KnownLanguages { get; set; } = new List<Languages>(); // Languages known by the character

    // Relationships
    public ICollection<InventoryItem>? InventoryItems { get; set; } // Inventory items

    // Constructor
    public CharacterBase()
    {
        // Ensure "Common" is added only if it's not already present
        if (!KnownLanguages.Contains(Languages.Common))
        {
            KnownLanguages.Add(Languages.Common);
        }
    }

    // Methods
    // Method to add known languages, ensuring "Common" is not duplicated
    public void AddKnownLanguage(Languages language)
    {
        if (!KnownLanguages.Contains(language))
        {
            KnownLanguages.Add(language);
        }
    }

    // Method to remove known languages
    public void RemoveKnownLanguage(Languages language)
    {
        KnownLanguages.Remove(language);
    }

    // Method to validate stats
    public void ValidateStats()
    {
        ValidateStat(Strength, "Strength");
        ValidateStat(Dexterity, "Dexterity");
        ValidateStat(Constitution, "Constitution");
        ValidateStat(Intelligence, "Intelligence");
        ValidateStat(Wisdom, "Wisdom");
        ValidateStat(Charisma, "Charisma");
    }

    // Method to validate a single stat
    protected void ValidateStat(int stat, string statName)
    {
        if (stat > MaxStatValue && !HasOver20Stats)
        {
            throw new FluentValidation.ValidationException($"{statName} cannot exceed {MaxStatValue} without special abilities.");
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