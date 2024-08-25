namespace Services;

public class PlayerCharacterService : IPlayerCharacterService
{
    // The repository is injected into the service
    private readonly IPlayerCharacterRepository _repository;

    // Constructor
    public PlayerCharacterService(IPlayerCharacterRepository repository)
    {
        _repository = repository;
    }

    // API methods
    public async Task<IEnumerable<PlayerCharacter>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<PlayerCharacter> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid character ID.", nameof(id));
        }

        var character = await _repository.GetByIdAsync(id);
        if (character == null)
        {
            throw new KeyNotFoundException($"PlayerCharacter with ID {id} could not be found.");
        }

        return character;
    }

    public async Task AddAsync(PlayerCharacter playerCharacter)
    {
        if (playerCharacter == null)
        {
            throw new ArgumentNullException(nameof(playerCharacter), "PlayerCharacter cannot be null.");
        }

        await _repository.AddAsync(playerCharacter);
    }

    public async Task UpdateAsync(PlayerCharacter playerCharacter)
    {
        if (playerCharacter == null)
        {
            throw new ArgumentNullException(nameof(playerCharacter), "PlayerCharacter cannot be null.");
        }

        if (!await _repository.ExistsAsync(playerCharacter.Id))
        {
            throw new KeyNotFoundException($"PlayerCharacter with ID {playerCharacter.Id} could not be found.");
        }

        await _repository.UpdateAsync(playerCharacter);
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid character ID.", nameof(id));
        }

        var character = await _repository.GetByIdAsync(id);
        if (character == null)
        {
            throw new KeyNotFoundException($"PlayerCharacter with ID {id} could not be found.");
        }

        await _repository.SoftDeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid character ID.", nameof(id));
        }

        return await _repository.ExistsAsync(id);
    }
}
