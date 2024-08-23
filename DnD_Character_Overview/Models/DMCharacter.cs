namespace Models;

public class DMCharacter : CharacterBase
{
    public string? Description { get; set; } // Additional description for the character

    // Special abilities and actions
    public string? LegendaryActions { get; set; } // Legendary actions for powerful NPCs or monsters
    public string? SpecialAbilities { get; set; } // Special abilities for the character


    // Additional properties or methods specific to DMCharacter can be added here
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