namespace Interfaces;

public interface IPlayerCharacterRepository
{
    Task<IEnumerable<PlayerCharacter>> GetAllAsync();
    Task<PlayerCharacter> GetByIdAsync(int id);
    Task AddAsync(PlayerCharacter playerCharacter);
    Task UpdateAsync(PlayerCharacter playerCharacter);
    Task SoftDeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task CleanupSoftDeletedAsync();
}