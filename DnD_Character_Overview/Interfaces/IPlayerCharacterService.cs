// using System.Collections.Generic;
// using System.Threading.Tasks;

namespace Services;

public interface IPlayerCharacterService
{
    Task<IEnumerable<PlayerCharacter>> GetAllAsync();
    Task<PlayerCharacter> GetByIdAsync(int id);
    Task AddAsync(PlayerCharacter playerCharacter);
    Task UpdateAsync(PlayerCharacter playerCharacter);
    Task SoftDeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}