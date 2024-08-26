namespace Services;

public interface IDMCharacterService
{
    Task<IEnumerable<DMCharacter>> GetAllAsync();
    Task<DMCharacter> GetByIdAsync(int id);
    Task AddAsync(DMCharacter dmCharacter);
    Task UpdateAsync(DMCharacter dmCharacter);
    Task SoftDeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
