using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet properties for each model
    public DbSet<PlayerCharacter> PlayerCharacters { get; set; }
    public DbSet<DMCharacter> DMCharacters { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<SharedInventory> SharedInventory { get; set; }

    // Override for OnModelCreating to configure relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Custom converter for List<Enums.Languages>
            var languagesConverter = new ValueConverter<List<Enums.Languages>, string>(
                v => string.Join(',', v.Select(l => l.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(l => Enum.Parse<Enums.Languages>(l)).ToList());

            var languagesComparer = new ValueComparer<List<Enums.Languages>>(
                (c1, c2) => c1.SequenceEqual(c2), // Equality logic
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Hash code logic
                c => c.ToList()); // Deep copy logic

            // Custom converter for List<Enums.DamageType>
            var damageTypeConverter = new ValueConverter<List<Enums.DamageType>, string>(
                v => string.Join(',', v.Select(d => d.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(d => Enum.Parse<Enums.DamageType>(d)).ToList());

            var damageTypeComparer = new ValueComparer<List<Enums.DamageType>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            // Custom converter for List<Enums.Conditions>
            var conditionsConverter = new ValueConverter<List<Enums.Conditions>, string>(
                v => string.Join(',', v.Select(c => c.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => Enum.Parse<Enums.Conditions>(c)).ToList());

            var conditionsComparer = new ValueComparer<List<Enums.Conditions>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            // Apply the custom converters and comparers to PlayerCharacter
            modelBuilder.Entity<PlayerCharacter>()
                .Property(e => e.KnownLanguages)
                .HasConversion(languagesConverter)
                .Metadata.SetValueComparer(languagesComparer);

            modelBuilder.Entity<PlayerCharacter>()
                .Property(e => e.Resistances)
                .HasConversion(damageTypeConverter)
                .Metadata.SetValueComparer(damageTypeComparer);

            modelBuilder.Entity<PlayerCharacter>()
                .Property(e => e.Weaknesses)
                .HasConversion(damageTypeConverter)
                .Metadata.SetValueComparer(damageTypeComparer);

            modelBuilder.Entity<PlayerCharacter>()
                .Property(e => e.Conditions)
                .HasConversion(conditionsConverter)
                .Metadata.SetValueComparer(conditionsComparer);

            // Apply the custom converters and comparers to DMCharacter
            modelBuilder.Entity<DMCharacter>()
                .Property(e => e.KnownLanguages)
                .HasConversion(languagesConverter)
                .Metadata.SetValueComparer(languagesComparer);

            modelBuilder.Entity<DMCharacter>()
                .Property(e => e.Resistances)
                .HasConversion(damageTypeConverter)
                .Metadata.SetValueComparer(damageTypeComparer);

            modelBuilder.Entity<DMCharacter>()
                .Property(e => e.Weaknesses)
                .HasConversion(damageTypeConverter)
                .Metadata.SetValueComparer(damageTypeComparer);

            modelBuilder.Entity<DMCharacter>()
                .Property(e => e.Conditions)
                .HasConversion(conditionsConverter)
                .Metadata.SetValueComparer(conditionsComparer);

            // Seeding data for PlayerCharacter
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

            // Seeding data for DMCharacter
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

            // Configuration for SharedInventory as a singleton
            modelBuilder.Entity<SharedInventory>()
                .HasData(new SharedInventory { Id = 1 });

            // Seed some InventoryItems for both PlayerCharacter and DMCharacter
            modelBuilder.Entity<InventoryItem>().HasData(
                new InventoryItem { Id = 1, ItemName = "Sword", Quantity = 1, Weight = 3.0m, PlayerCharacterId = 1 },
                new InventoryItem { Id = 2, ItemName = "Shield", Quantity = 1, Weight = 6.0m, PlayerCharacterId = 2 },
                new InventoryItem { Id = 3, ItemName = "Health Potion", Quantity = 5, Weight = 0.5m, DMCharacterId = 1 }
            );

            // SharedInventory has many InventoryItems
            modelBuilder.Entity<SharedInventory>()
                .HasMany(s => s.InventoryItems)
                .WithOne(i => i.SharedInventory)
                .HasForeignKey(i => i.SharedInventoryId);
    }
}