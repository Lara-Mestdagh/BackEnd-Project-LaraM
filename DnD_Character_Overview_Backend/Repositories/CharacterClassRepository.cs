using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class CharacterClassRepository : ICharacterClassRepository
{
    private readonly ApplicationDbContext _context;

    public CharacterClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CharacterClass>> GetClassesByCharacterIdAsync(int characterId)
    {
        return await _context.CharacterClasses
                             .Where(cc => cc.PlayerCharacterId == characterId)
                             .ToListAsync();
    }

    public async Task<IEnumerable<CharacterClass>> GetClassesByDMCharacterIdAsync(int dmCharacterId)
    {
        return await _context.CharacterClasses
                             .Where(cc => cc.DMCharacterId == dmCharacterId)
                             .ToListAsync();
    }

    public async Task AddClassesAsync(int characterId, IEnumerable<CharacterClass> classes)
    {
        foreach (var characterClass in classes)
        {
            characterClass.PlayerCharacterId = characterId;
            _context.CharacterClasses.Add(characterClass);
        }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClassesAsync(int characterId, IEnumerable<CharacterClass> classes)
    {
        var existingClasses = await GetClassesByCharacterIdAsync(characterId);
        _context.CharacterClasses.RemoveRange(existingClasses);
        await AddClassesAsync(characterId, classes);
    }

    public async Task DeleteClassesByCharacterIdAsync(int characterId)
    {
        var classes = await GetClassesByCharacterIdAsync(characterId);
        _context.CharacterClasses.RemoveRange(classes);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClassesByDMCharacterIdAsync(int dmCharacterId)
    {
        var classes = await GetClassesByDMCharacterIdAsync(dmCharacterId);
        _context.CharacterClasses.RemoveRange(classes);
        await _context.SaveChangesAsync();
    }
}