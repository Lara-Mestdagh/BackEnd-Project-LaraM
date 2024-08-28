namespace Interfaces;

public interface ICharacterClassRepository
{
    Task<IEnumerable<CharacterClass>> GetClassesByPlayerCharacterIdAsync(int characterId);
    Task<IEnumerable<CharacterClass>> GetClassesByDMCharacterIdAsync(int dmCharacterId); 
    Task AddClassesToPlayerCharacterAsync(int characterId, IEnumerable<CharacterClass> classes);
    Task AddClassesToDMCharacterAsync(int dmCharacterId, IEnumerable<CharacterClass> classes);
    Task UpdateClassesAsync(IEnumerable<CharacterClass> characterClasses);
    Task DeleteClassesByPlayerCharacterIdAsync(int characterId);
    Task DeleteClassesByDMCharacterIdAsync(int dmCharacterId); 
}
