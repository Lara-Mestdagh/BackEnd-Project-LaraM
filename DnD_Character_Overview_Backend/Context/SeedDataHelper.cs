using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Context;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        SeedPlayerCharacters(modelBuilder);
        SeedDMCharacters(modelBuilder);
        SeedCharacterClasses(modelBuilder);
        SeedInventoryItems(modelBuilder);
    }

    private static void SeedPlayerCharacters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerCharacter>().HasData(
            new PlayerCharacter
            {
                Id = 1,
                Name = "Arthas",
                Race = "Human",
                PlayerName = "John",
                Strength = 16,
                Dexterity = 14,
                Constitution = 14,
                Intelligence = 12,
                Wisdom = 10,
                Charisma = 16,
                MaxHP = 40,
                CurrentHP = 35,
                TempHP = 0,
                ArmorClass = 16,
                WalkingSpeed = 30,
                IsAlive = true,
                KnownLanguages = new List<Enums.Languages> { Enums.Languages.Common },
                Resistances = new List<Enums.DamageType> { Enums.DamageType.Bludgeoning, Enums.DamageType.Cold },
                Weaknesses = new List<Enums.DamageType> { Enums.DamageType.Fire, Enums.DamageType.Lightning }
            },
            new PlayerCharacter
            {
                Id = 2,
                Name = "Luna",
                Race = "Half-Elf",
                PlayerName = "Jane",
                Strength = 10,
                Dexterity = 16,
                Constitution = 12,
                Intelligence = 14,
                Wisdom = 14,
                Charisma = 18,
                MaxHP = 30,
                CurrentHP = 25,
                TempHP = 0,
                ArmorClass = 14,
                WalkingSpeed = 30,
                SwimmingSpeed = 30,
                DarkvisionRange = 60,
                IsAlive = true,
                KnownLanguages = new List<Enums.Languages> { Enums.Languages.Common, Enums.Languages.Elvish },
                Resistances = new List<Enums.DamageType> { Enums.DamageType.Radiant, Enums.DamageType.Thunder },
                Weaknesses = new List<Enums.DamageType> { Enums.DamageType.Necrotic, Enums.DamageType.Poison }
            },
            new PlayerCharacter
            {
                Id = 3,
                Name = "Thorin",
                Race = "Dwarf",
                PlayerName = "Peter",
                Strength = 18,
                Dexterity = 10,
                Constitution = 16,
                Intelligence = 10,
                Wisdom = 12,
                Charisma = 14,
                MaxHP = 50,
                CurrentHP = 50,
                TempHP = 0,
                ArmorClass = 18,
                WalkingSpeed = 25,
                IsAlive = true,
                KnownLanguages = new List<Enums.Languages> { Enums.Languages.Common, Enums.Languages.Dwarvish },
                Resistances = new List<Enums.DamageType> { Enums.DamageType.Poison },
                Weaknesses = new List<Enums.DamageType> { Enums.DamageType.Psychic }
            },
            new PlayerCharacter
            {
                Id = 4,
                Name = "Sylvanas",
                Race = "Elf",
                PlayerName = "Mary",
                Strength = 12,
                Dexterity = 20,
                Constitution = 10,
                Intelligence = 14,
                Wisdom = 16,
                Charisma = 12,
                MaxHP = 28,
                CurrentHP = 28,
                TempHP = 0,
                ArmorClass = 15,
                WalkingSpeed = 35,
                DarkvisionRange = 60,
                IsAlive = true,
                KnownLanguages = new List<Enums.Languages> { Enums.Languages.Common, Enums.Languages.Elvish },
                Resistances = new List<Enums.DamageType> { Enums.DamageType.Necrotic },
                Weaknesses = new List<Enums.DamageType> { Enums.DamageType.Fire }
            }
        );
    }

    private static void SeedDMCharacters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DMCharacter>().HasData(
            new DMCharacter
            {
                Id = 1,
                Name = "Zugthrok the Mighty",
                Race = "Orc",
                Strength = 18,
                Dexterity = 14,
                Constitution = 16,
                Intelligence = 8,
                Wisdom = 10,
                Charisma = 12,
                MaxHP = 100,
                CurrentHP = 100,
                TempHP = 0,
                ArmorClass = 17,
                WalkingSpeed = 30,
                IsAlive = true,
                KnownLanguages = new List<Enums.Languages> { Enums.Languages.Common, Enums.Languages.Orc },
                Resistances = new List<Enums.DamageType> { Enums.DamageType.Bludgeoning, Enums.DamageType.Poison },
                Weaknesses = new List<Enums.DamageType> { Enums.DamageType.Psychic, Enums.DamageType.Fire },
                LegendaryActions = "Legendary Slash, Intimidating Roar",
                SpecialAbilities = "Darkvision, Fury of Blows"
            },
            new DMCharacter
            {
                Id = 2,
                Name = "Elandril the Wise",
                Race = "High Elf",
                Strength = 12,
                Dexterity = 14,
                Constitution = 12,
                Intelligence = 20,
                Wisdom = 18,
                Charisma = 16,
                MaxHP = 60,
                CurrentHP = 60,
                TempHP = 0,
                ArmorClass = 15,
                WalkingSpeed = 30,
                IsAlive = true,
                KnownLanguages = new List<Enums.Languages> { Enums.Languages.Common, Enums.Languages.Elvish, Enums.Languages.Draconic },
                Resistances = new List<Enums.DamageType> { Enums.DamageType.Fire, Enums.DamageType.Psychic },
                Weaknesses = new List<Enums.DamageType> { Enums.DamageType.Necrotic, Enums.DamageType.Poison },
                LegendaryActions = "Arcane Barrage, Teleport",
                SpecialAbilities = "Truesight, Spellcasting"
            }
        );
    }

    private static void SeedCharacterClasses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterClass>().HasData(
            // Classes for Player Characters
            new CharacterClass { Id = 1, PlayerCharacterId = 1, DMCharacterId = null, ClassName = "Warrior", Level = 3 },
            new CharacterClass { Id = 2, PlayerCharacterId = 2, DMCharacterId = null, ClassName = "Mage", Level = 4 },
            new CharacterClass { Id = 3, PlayerCharacterId = 3, DMCharacterId = null, ClassName = "Paladin", Level = 5 },
            new CharacterClass { Id = 4, PlayerCharacterId = 4, DMCharacterId = null, ClassName = "Ranger", Level = 4 },
            
            // Ensure that DM Characters have unique IDs as well
            new CharacterClass { Id = 5, PlayerCharacterId = null, DMCharacterId = 1, ClassName = "Barbarian", Level = 6 },
            new CharacterClass { Id = 6, PlayerCharacterId = null, DMCharacterId = 2, ClassName = "Wizard", Level = 7 }
        );
    }

    private static void SeedInventoryItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>().HasData(
            // Player Character 1
            new InventoryItem { Id = 1, ItemName = "Longsword", Quantity = 1, Weight = 3.0m, PlayerCharacterId = 1 },
            new InventoryItem { Id = 2, ItemName = "Chainmail Armor", Quantity = 1, Weight = 55.0m, PlayerCharacterId = 1 },
            new InventoryItem { Id = 3, ItemName = "Health Potion", Quantity = 2, Weight = 0.5m, PlayerCharacterId = 1 },
            new InventoryItem { Id = 4, ItemName = "Torch", Quantity = 5, Weight = 1.0m, PlayerCharacterId = 1 },

            // Player Character 2
            new InventoryItem { Id = 5, ItemName = "Dagger", Quantity = 2, Weight = 1.0m, PlayerCharacterId = 2 },
            new InventoryItem { Id = 6, ItemName = "Leather Armor", Quantity = 1, Weight = 10.0m, PlayerCharacterId = 2 },
            new InventoryItem { Id = 7, ItemName = "Mana Potion", Quantity = 3, Weight = 0.4m, PlayerCharacterId = 2 },

            // Player Character 3
            new InventoryItem { Id = 8, ItemName = "Warhammer", Quantity = 1, Weight = 2.5m, PlayerCharacterId = 3 },
            new InventoryItem { Id = 9, ItemName = "Scale Mail", Quantity = 1, Weight = 45.0m, PlayerCharacterId = 3 },
            new InventoryItem { Id = 10, ItemName = "Health Potion", Quantity = 3, Weight = 0.5m, PlayerCharacterId = 3 },

            // Player Character 4
            new InventoryItem { Id = 11, ItemName = "Bow", Quantity = 1, Weight = 2.0m, PlayerCharacterId = 4 },
            new InventoryItem { Id = 12, ItemName = "Quiver of Arrows", Quantity = 20, Weight = 1.5m, PlayerCharacterId = 4 },
            new InventoryItem { Id = 13, ItemName = "Cloak of Invisibility", Quantity = 1, Weight = 1.0m, PlayerCharacterId = 4 },

            // DM Character 1
            new InventoryItem { Id = 14, ItemName = "Orcish Great Axe", Quantity = 1, Weight = 7.0m, DMCharacterId = 1 },
            new InventoryItem { Id = 15, ItemName = "Health Potion", Quantity = 4, Weight = 0.5m, DMCharacterId = 1 },

            // DM Character 2
            new InventoryItem { Id = 16, ItemName = "Spellbook", Quantity = 1, Weight = 5.0m, DMCharacterId = 2 },
            new InventoryItem { Id = 17, ItemName = "Wand of Fireballs", Quantity = 1, Weight = 2.0m, DMCharacterId = 2 },

            // Shared Inventory
            new InventoryItem { Id = 18, ItemName = "Rope", Quantity = 1, Weight = 10.0m, SharedInventoryId = 1 },
            new InventoryItem { Id = 19, ItemName = "Tent", Quantity = 1, Weight = 20.0m, SharedInventoryId = 1 },
            new InventoryItem { Id = 20, ItemName = "Food Rations", Quantity = 10, Weight = 1.0m, SharedInventoryId = 1 },
            new InventoryItem { Id = 21, ItemName = "Water Flask", Quantity = 4, Weight = 2.0m, SharedInventoryId = 1 }
        );
    }
}
