namespace Services;

public class DMCharacterService : IDMCharacterService
{
    // The repository is injected into the service
    private readonly IDMCharacterRepository _repository;

    // Constructor
    public DMCharacterService(IDMCharacterRepository repository)
    {
        _repository = repository;
    }

    // API methods
    public async Task<IEnumerable<DMCharacter>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<DMCharacter> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(DMCharacter dmCharacter)
    {
        if (dmCharacter == null)
        {
            throw new ArgumentNullException(nameof(dmCharacter), "DMCharacter cannot be null.");
        }

        await _repository.AddAsync(dmCharacter);
    }

    public async Task UpdateAsync(DMCharacter dmCharacter)
    {
        if (dmCharacter == null)
        {
            throw new ArgumentNullException(nameof(dmCharacter), "DMCharacter cannot be null.");
        }

        if (!await _repository.ExistsAsync(dmCharacter.Id))
        {
            throw new KeyNotFoundException($"DM Character with ID {dmCharacter.Id} could not be found.");
        }

        await _repository.UpdateAsync(dmCharacter);
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException("Invalid character ID.", nameof(id));
        }

        var character = await GetByIdAsync(id);
        if (character == null)
        {
            throw new KeyNotFoundException($"DM Character with ID {id} could not be found.");
        }

        await _repository.SoftDeleteAsync(id);
    }

    // Check if a DM character exists
    public async Task<bool> ExistsAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException("Invalid character ID.", nameof(id));
        }

        return await _repository.ExistsAsync(id);
    }
}