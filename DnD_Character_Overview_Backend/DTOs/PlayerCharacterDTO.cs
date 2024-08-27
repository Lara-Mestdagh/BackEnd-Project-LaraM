namespace DTOs;

public class PlayerCharacterDTO
{
    // Base properties inherited from CharacterBase
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Race { get; set; }

    public string? ImagePath { get; set; }

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }

    public int MaxHP { get; set; }
    public int? CurrentHP { get; set; }
    public int TempHP { get; set; }
    public int ArmorClass { get; set; }

    public int WalkingSpeed { get; set; }
    public int? FlyingSpeed { get; set; }
    public int? SwimmingSpeed { get; set; }
    public int? DarkvisionRange { get; set; }

    public int InitiativeModifier { get; set; }

    public List<string> Resistances { get; set; } = new List<string>();
    public List<string> Weaknesses { get; set; } = new List<string>();

    public List<string> Conditions { get; set; } = new List<string>();

    public bool IsAlive { get; set; }
    public bool HasOver20Stats { get; set; }

    public List<string> KnownLanguages { get; set; } = new List<string>();

    // Player-specific properties
    public string? PlayerName { get; set; }

    // Optional properties related to character classes and inventory
    public ICollection<CharacterClassDTO> CharacterClasses { get; set; } = new List<CharacterClassDTO>();
}