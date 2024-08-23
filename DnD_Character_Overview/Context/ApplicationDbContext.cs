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

        // Custom converter for List<Enums.DamageType>
        var damageTypeConverter = new ValueConverter<List<Enums.DamageType>, string>(
            v => string.Join(',', v.Select(d => d.ToString())),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(d => Enum.Parse<Enums.DamageType>(d)).ToList());

        // Custom comparer for List<Enums.Languages>
        var languagesComparer = new ValueComparer<List<Enums.Languages>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        // Custom comparer for List<Enums.DamageType>
        var damageTypeComparer = new ValueComparer<List<Enums.DamageType>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        // Configurations for PlayerCharacter
        modelBuilder.Entity<PlayerCharacter>()
            .HasMany(c => c.InventoryItems)
            .WithOne(i => i.PlayerCharacter)
            .HasForeignKey(i => i.PlayerCharacterId);

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

        // Configurations for DMCharacter
        modelBuilder.Entity<DMCharacter>()
            .HasMany(c => c.InventoryItems)
            .WithOne(i => i.DMCharacter)
            .HasForeignKey(i => i.DMCharacterId);

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

        // Configuration for SharedInventory as a singleton
        modelBuilder.Entity<SharedInventory>()
            .HasData(new SharedInventory { Id = 1 });

        // SharedInventory has many InventoryItems
        modelBuilder.Entity<SharedInventory>()
            .HasMany(s => s.InventoryItems)
            .WithOne(i => i.SharedInventory)
            .HasForeignKey(i => i.SharedInventoryId);
    }
}