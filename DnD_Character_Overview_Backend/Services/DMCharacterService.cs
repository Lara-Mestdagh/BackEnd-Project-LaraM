using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Services;

public class DMCharacterService : IDMCharacterService
{
    private readonly IDMCharacterRepository _repository;
    private readonly ICharacterClassRepository _classRepository;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30); // Current cache duration is 30 minutes

    public DMCharacterService(IDMCharacterRepository repository, ICharacterClassRepository classRepository, IMemoryCache cache)
    {
        _repository = repository;
        _classRepository = classRepository;
        _cache = cache;
    }

    // API methods
    public async Task<IEnumerable<DMCharacter>> GetAllAsync()
    {
        var cacheKey = "AllDMCharacters";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<DMCharacter>? characters) || characters == null)
        {
            characters = await _repository.GetAllAsync();

            // Fetch and include character classes
            foreach (var character in characters)
            {
                character.CharacterClasses = (await _classRepository.GetClassesByDMCharacterIdAsync(character.Id)).ToList();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            };

            _cache.Set(cacheKey, characters, cacheEntryOptions);
        }

        return characters;
    }

    public async Task<DMCharacter> GetByIdAsync(int id)
    {
        var cacheKey = $"DMCharacter_{id}";

        if (!_cache.TryGetValue(cacheKey, out DMCharacter? character) || character == null)
        {
            character = await _repository.GetByIdAsync(id);

            if (character != null)
            {
                // Fetch and include character classes
                character.CharacterClasses = (await _classRepository.GetClassesByDMCharacterIdAsync(character.Id)).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

                _cache.Set(cacheKey, character, cacheEntryOptions);
            }
        }

        return character;
    }

    public async Task AddAsync(DMCharacter dmCharacter)
    {
        if (dmCharacter == null)
        {
            throw new ArgumentNullException(nameof(dmCharacter), "DMCharacter cannot be null.");
        }

        await _repository.AddAsync(dmCharacter);

        if (dmCharacter.CharacterClasses != null && dmCharacter.CharacterClasses.Any())
        {
            await _classRepository.AddClassesToDMCharacterAsync(dmCharacter.Id, dmCharacter.CharacterClasses);
        }

        // Invalidate cache for the collection to ensure the new character appears in future queries
        _cache.Remove("AllDMCharacters");
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

        if (dmCharacter.CharacterClasses != null && dmCharacter.CharacterClasses.Any())
        {
            await _classRepository.UpdateClassesAsync(dmCharacter.CharacterClasses);
        }

        // Invalidate the cache for both specific item and all items
        var cacheKey = $"DMCharacter_{dmCharacter.Id}";
        _cache.Remove(cacheKey);
        _cache.Remove("AllDMCharacters");
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

        // Invalidate the cache for both specific item and all items
        var cacheKey = $"DMCharacter_{id}";
        _cache.Remove(cacheKey);
        _cache.Remove("AllDMCharacters");
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
