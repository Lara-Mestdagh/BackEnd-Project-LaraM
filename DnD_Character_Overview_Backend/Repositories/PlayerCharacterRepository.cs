using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class PlayerCharacterRepository : IPlayerCharacterRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerCharacterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // API methods
    public async Task<IEnumerable<PlayerCharacter>> GetAllAsync()
    {
        return await _context.PlayerCharacters.ToListAsync();
    }

    public async Task<PlayerCharacter> GetByIdAsync(int id)
    {
        // Check if the character exists and id is valid
        if (id <= 0)
        {
            return null; 
        }

        return await _context.PlayerCharacters.FirstOrDefaultAsync(pc => pc.Id == id);
    }

    public async Task AddAsync(PlayerCharacter playerCharacter)
    {
        if (playerCharacter == null)
        {
            throw new ArgumentNullException(nameof(playerCharacter), "PlayerCharacter cannot be null.");
        }

        await _context.PlayerCharacters.AddAsync(playerCharacter);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PlayerCharacter playerCharacter)
    {
        if (playerCharacter == null)
        {
            return; 
        }

        _context.PlayerCharacters.Update(playerCharacter);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var character = await GetByIdAsync(id);
        if (character == null)
        {
            return;  
        }

        character.IsAlive = false;
        await UpdateAsync(character);
    }

    // Check if a character exists
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PlayerCharacters.AnyAsync(e => e.Id == id);
    }

    public async Task CleanupSoftDeletedAsync()
    {
        // Fetch all characters marked as soft deleted aka isAlive = false
        var characters = await _context.PlayerCharacters
            .Where(pc => pc.IsAlive == false)
            .ToListAsync();

        if (characters.Any())
        {
            _context.PlayerCharacters.RemoveRange(characters);
            await _context.SaveChangesAsync();

            // Log the cleanup
            Console.WriteLine($"Soft deleted characters cleanup completed. {characters.Count} characters removed.");
        }
        else
        {
            Console.WriteLine("No soft-deleted characters found for cleanup.");
        }
    }
}