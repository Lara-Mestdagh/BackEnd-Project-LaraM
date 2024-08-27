namespace Interfaces;

public interface ICharacterClassRepository
{
    Task<IEnumerable<CharacterClass>> GetClassesByCharacterIdAsync(int characterId);
    Task<IEnumerable<CharacterClass>> GetClassesByDMCharacterIdAsync(int dmCharacterId); 
    Task AddClassesAsync(int characterId, IEnumerable<CharacterClass> classes);
    Task UpdateClassesAsync(int characterId, IEnumerable<CharacterClass> classes);
    Task DeleteClassesByCharacterIdAsync(int characterId);
    Task DeleteClassesByDMCharacterIdAsync(int dmCharacterId); 
}
