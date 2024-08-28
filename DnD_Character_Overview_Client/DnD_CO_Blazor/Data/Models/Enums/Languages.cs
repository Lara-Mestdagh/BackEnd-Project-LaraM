using System.Text.Json.Serialization;

namespace Enums;


[JsonConverter(typeof(CaseInsensitiveEnumConverterFactory))]

public enum Languages
    {
        // Standard Languages
        Common,       // Humans
        Dwarvish,     // Dwarves
        Elvish,       // Elves
        Giant,        // Ogres, giants
        Gnomish,      // Gnomes
        Goblin,       // Goblinoids
        Halfling,     // Halflings
        Orc,          // Orcs

        // Exotic Languages
        Abyssal,      // Demons (Script: Infernal)
        Celestial,    // Celestials (Script: Celestial)
        Draconic,     // Dragons, dragonborn (Script: Draconic)
        DeepSpeech,   // Aboleths, cloakers (No script)
        Infernal,     // Devils (Script: Infernal)
        Primordial,   // Elementals (Script: Dwarvish)
        Sylvan,       // Fey creatures (Script: Elvish)
        Undercommon   // Underworld traders (Script: Elvish)
    }