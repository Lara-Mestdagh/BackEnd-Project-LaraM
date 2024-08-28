using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class CharacterClassRepository : ICharacterClassRepository
{
    private readonly ApplicationDbContext _context;

    public CharacterClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Method to get classes by Player Character Id
    public async Task<IEnumerable<CharacterClass>> GetClassesByPlayerCharacterIdAsync(int playerCharacterId)
    {
        return await _context.CharacterClasses
                             .Where(cc => cc.PlayerCharacterId == playerCharacterId)
                             .ToListAsync();
    }

    // Method to get classes by DM Character Id
    public async Task<IEnumerable<CharacterClass>> GetClassesByDMCharacterIdAsync(int dmCharacterId)
    {
        return await _context.CharacterClasses
                             .Where(cc => cc.DMCharacterId == dmCharacterId)
                             .ToListAsync();
    }

    // Method to add classes to a Player Character
    public async Task AddClassesToPlayerCharacterAsync(int playerCharacterId, IEnumerable<CharacterClass> classes)
    {
        foreach (var characterClass in classes)
        {
            characterClass.PlayerCharacterId = playerCharacterId;  // Set PlayerCharacterId
            characterClass.DMCharacterId = null; // Ensure DMCharacterId is null
            _context.CharacterClasses.Add(characterClass);
        }
        await _context.SaveChangesAsync();
    }

    // Method to add classes to a DM Character
    public async Task AddClassesToDMCharacterAsync(int dmCharacterId, IEnumerable<CharacterClass> classes)
    {
        foreach (var characterClass in classes)
        {
            characterClass.DMCharacterId = dmCharacterId;  // Set DMCharacterId
            characterClass.PlayerCharacterId = null; // Ensure PlayerCharacterId is null
            _context.CharacterClasses.Add(characterClass);
        }
        await _context.SaveChangesAsync();
    }

    // Method to update classes, ensuring it handles both Player and DM characters correctly
    public async Task UpdateClassesAsync(IEnumerable<CharacterClass> characterClasses)
    {
        foreach (var characterClass in characterClasses)
        {
            if (await _context.CharacterClasses.AnyAsync(cc => cc.Id == characterClass.Id))
            {
                _context.Entry(characterClass).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(characterClass).State = EntityState.Added;
            }
        }

        await _context.SaveChangesAsync();
    }

    // Method to delete classes by Player Character Id
    public async Task DeleteClassesByPlayerCharacterIdAsync(int playerCharacterId)
    {
        var classes = await GetClassesByPlayerCharacterIdAsync(playerCharacterId);
        _context.CharacterClasses.RemoveRange(classes);
        await _context.SaveChangesAsync();
    }

    // Method to delete classes by DM Character Id
    public async Task DeleteClassesByDMCharacterIdAsync(int dmCharacterId)
    {
        var classes = await GetClassesByDMCharacterIdAsync(dmCharacterId);
        _context.CharacterClasses.RemoveRange(classes);
        await _context.SaveChangesAsync();
    }
}
