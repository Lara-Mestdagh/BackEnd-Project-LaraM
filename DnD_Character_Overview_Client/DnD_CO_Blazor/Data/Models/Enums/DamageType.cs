using System.Text.Json.Serialization;

namespace Enums;


[JsonConverter(typeof(CaseInsensitiveEnumConverterFactory))]

public enum DamageType
{
    Acid,
    Bludgeoning,
    Cold,
    Fire,
    Force,
    Lightning,
    Necrotic,
    Piercing,
    Poison,
    Psychic,
    Radiant,
    Slashing,
    Thunder
}