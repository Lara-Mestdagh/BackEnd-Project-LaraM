namespace Models;

public class PlayerCharacter : CharacterBase
{
    // Player-specific properties
    [Required]
    [StringLength(255)]
    public string? PlayerName { get; set; } // IRL player properties

    // Additional properties or methods specific to PlayerCharacter can be added here
}

// From base class
// Properties inherited from CharacterBase:
// - Id (int)
// - Name (string?)
// - Race (string?)
// - ImagePath (string?)
// - Strength (int)
// - Dexterity (int)
// - Constitution (int)
// - Intelligence (int)
// - Wisdom (int)
// - Charisma (int)
// - MaxHP (int)
// - CurrentHP (int?)
// - TempHP (int)
// - ArmorClass (int)
// - WalkingSpeed (int)
// - FlyingSpeed (int?)
// - SwimmingSpeed (int?)
// - DarkvisionRange (int?)
// - InitiativeModifier (int)
// - Resistances (List<string>)
// - Weaknesses (List<string>)
// - Conditions (string?)
// - IsAlive (bool)
// - HasOver20Stats (bool)
// - KnownLanguages (List<string>)
// - CharacterClasses (ICollection<CharacterClass>?)
// - InventoryItems (ICollection<InventoryItem>?)

// Methods inherited from CharacterBase:
// - void ValidateStats()
// - void ValidateStat(int stat, string statName)
// - int GetModifier(int stat)
// - int StrengthModifier
// - int DexterityModifier
// - int ConstitutionModifier
// - int IntelligenceModifier
// - int WisdomModifier
// - int CharismaModifier