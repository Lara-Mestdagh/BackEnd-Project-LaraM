using System.Text.Json.Serialization;

namespace Enums;


[JsonConverter(typeof(CaseInsensitiveEnumConverterFactory))]
public enum Conditions
{
    Blinded,
    Charmed,
    Deafened,
    Exhaustion,
    Frightened,
    Grappled,
    Incapacitated,
    Invisible,
    Paralyzed,
    Petrified,
    Poisoned,
    Prone,
    Restrained,
    Stunned,
    Unconscious
    // Add any other conditions as needed
}
