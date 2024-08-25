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
            }
        );
    }

    private static void SeedInventoryItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>().HasData(
            new InventoryItem { Id = 1, ItemName = "Sword", Quantity = 1, Weight = 3.0m, PlayerCharacterId = 1 },
            new InventoryItem { Id = 2, ItemName = "Shield", Quantity = 1, Weight = 6.0m, PlayerCharacterId = 2 },
            new InventoryItem { Id = 3, ItemName = "Health Potion", Quantity = 5, Weight = 0.5m, DMCharacterId = 1 }
        );
    }
}