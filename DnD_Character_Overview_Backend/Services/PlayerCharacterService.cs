using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Services;

public class PlayerCharacterService : IPlayerCharacterService
{
    private readonly IPlayerCharacterRepository _repository;
    private readonly ICharacterClassRepository _classRepository;
    private readonly IMemoryCache _cache; 
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30); 

    // Constructor
    public PlayerCharacterService(IPlayerCharacterRepository repository, ICharacterClassRepository classRepository, IMemoryCache cache)
    {
        _repository = repository;
        _classRepository = classRepository;
        _cache = cache;
    }

    // API methods
    public async Task<IEnumerable<PlayerCharacter>> GetAllAsync()
    {
        var cacheKey = "AllPlayerCharacters";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<PlayerCharacter>? characters) || characters == null)
        {
            characters = await _repository.GetAllAsync();
            
            // Fetch and include character classes for each character
            foreach (var character in characters)
            {
                character.CharacterClasses = (await _classRepository.GetClassesByCharacterIdAsync(character.Id)).ToList();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            };

            _cache.Set(cacheKey, characters, cacheEntryOptions);
        }

        return characters;
    }

    public async Task<PlayerCharacter> GetByIdAsync(int id)
    {
        var cacheKey = $"PlayerCharacter_{id}";

        if (!_cache.TryGetValue(cacheKey, out PlayerCharacter? character) || character == null)
        {
            character = await _repository.GetByIdAsync(id);

            if (character != null)
            {
                // Fetch and include character classes for the specific character
                character.CharacterClasses = (await _classRepository.GetClassesByCharacterIdAsync(character.Id)).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

                _cache.Set(cacheKey, character, cacheEntryOptions);
            }
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

        // Optionally add classes to CharacterClassRepository if provided
        if (playerCharacter.CharacterClasses != null && playerCharacter.CharacterClasses.Any())
        {
            await _classRepository.AddClassesAsync(playerCharacter.Id, playerCharacter.CharacterClasses);
        }

        // Invalidate cache for "AllPlayerCharacters" to ensure it is refreshed
        _cache.Remove("AllPlayerCharacters");
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

        // Optionally update classes in CharacterClassRepository if provided
        if (playerCharacter.CharacterClasses != null && playerCharacter.CharacterClasses.Any())
        {
            await _classRepository.UpdateClassesAsync(playerCharacter.Id, playerCharacter.CharacterClasses);
        }

        // Invalidate the cache for both specific item and all items
        var cacheKey = $"PlayerCharacter_{playerCharacter.Id}";
        _cache.Remove(cacheKey);
        _cache.Remove("AllPlayerCharacters");
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

        // Invalidate cache for both specific item and all items
        var cacheKey = $"PlayerCharacter_{id}";
        _cache.Remove(cacheKey);
        _cache.Remove("AllPlayerCharacters");
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
