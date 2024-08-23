using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class PlayerCharacterRepository : IPlayerCharacterRepository
{
    // Database context
    private readonly ApplicationDbContext _context;

    // Constructor
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
            return null;  // Handle this at the service or controller level
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
            return;  // Same here, generally handle null validation before reaching the repository
        }

        _context.PlayerCharacters.Update(playerCharacter);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var character = await GetByIdAsync(id);
        if (character == null)
        {
            return;  // Again, handle this null case at a higher level
        }

        character.IsAlive = false;
        await UpdateAsync(character);  // Reuse the update method
    }

    // Check if a character exists
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PlayerCharacters.AnyAsync(e => e.Id == id);
    }
}