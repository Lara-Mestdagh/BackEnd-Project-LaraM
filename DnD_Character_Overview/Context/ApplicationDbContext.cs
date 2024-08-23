using Microsoft.EntityFrameworkCore;
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

        // Configurations for PlayerCharacter
        // PlayerCharacter has many InventoryItems
        modelBuilder.Entity<PlayerCharacter>()
            .HasMany(c => c.InventoryItems)
            .WithOne(i => i.PlayerCharacter)
            .HasForeignKey(i => i.PlayerCharacterId);

        // Configurations for DMCharacter
        // DMCharacter has many InventoryItems
        modelBuilder.Entity<DMCharacter>()
            .HasMany(c => c.InventoryItems)
            .WithOne(i => i.DMCharacter)
            .HasForeignKey(i => i.DMCharacterId);

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
