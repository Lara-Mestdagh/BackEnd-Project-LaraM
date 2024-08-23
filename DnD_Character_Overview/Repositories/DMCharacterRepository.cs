using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class DMCharacterRepository : IDMCharacterRepository
{
    // Database context
    private readonly ApplicationDbContext _context;

    // Constructor
    public DMCharacterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // API methods
    public async Task<IEnumerable<DMCharacter>> GetAllAsync()
    {
        return await _context.DMCharacters.ToListAsync();
    }

    public async Task<DMCharacter> GetByIdAsync(int id)
    {
        if (id == 0)
        {
            return null;
        }

        return await _context.DMCharacters.FirstOrDefaultAsync(dmc => dmc.Id == id);
    }

    public async Task AddAsync(DMCharacter dmCharacter)
    {
        if (dmCharacter == null)
        {
            return;
        }

        await _context.DMCharacters.AddAsync(dmCharacter);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DMCharacter dmCharacter)
    {
        if (dmCharacter == null)
        {
            return;
        }

        _context.DMCharacters.Update(dmCharacter);
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
        await UpdateAsync(character);  // Reuse the update method
        
    }

    // Check if a character exists
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.DMCharacters.AnyAsync(e => e.Id == id);
    }
}