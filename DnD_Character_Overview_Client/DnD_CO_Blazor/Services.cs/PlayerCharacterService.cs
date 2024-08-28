using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Services;

public class PlayerCharacterService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PlayerCharacterService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Methods for Player Characters
    public async Task<List<PlayerCharacter>> GetPlayerCharactersAsync()
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.GetFromJsonAsync<List<PlayerCharacter>>("player-characters");

        if (response == null)
        {
            Console.WriteLine("No player characters found.");
            return new List<PlayerCharacter>();
        }

        return response;
    }

    public async Task<PlayerCharacter> GetPlayerCharacterByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        return await client.GetFromJsonAsync<PlayerCharacter>($"player-characters/{id}");
    }

    public async Task AddPlayerCharacterAsync(PlayerCharacter character)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");

        // Convert your PlayerCharacter to PlayerCharacterDTO if needed
        var dto = new PlayerCharacterDTO
        {
            PlayerName = character.PlayerName,
            Name = character.Name,
            Race = character.Race,
            ImagePath = character.ImagePath,
            Strength = character.Strength,
            Dexterity = character.Dexterity,
            Constitution = character.Constitution,
            Intelligence = character.Intelligence,
            Wisdom = character.Wisdom,
            Charisma = character.Charisma,
            CurrentHP = character.CurrentHP,
            MaxHP = character.MaxHP,
            TempHP = character.TempHP,
            ArmorClass = character.ArmorClass,
            WalkingSpeed = character.WalkingSpeed,
            FlyingSpeed = character.FlyingSpeed,
            SwimmingSpeed = character.SwimmingSpeed,
            DarkvisionRange = character.DarkvisionRange,
            IsAlive = character.IsAlive,
            KnownLanguages = character.KnownLanguages.Select(lang => lang.ToString()).ToList(),  // Convert Enums to Strings
            Resistances = character.Resistances.Select(res => res.ToString()).ToList(),         // Convert Enums to Strings
            Weaknesses = character.Weaknesses.Select(weak => weak.ToString()).ToList(),         // Convert Enums to Strings
            HasOver20Stats = character.HasOver20Stats,
            CharacterClasses = character.CharacterClasses.Select(c => new CharacterClassDTO
            {
                Id = c.Id,
                ClassName = c.ClassName,
                Level = c.Level,
                PlayerCharacterId = c.PlayerCharacterId,
                DMCharacterId = c.DMCharacterId ?? null
            }).ToList()
        };

        var response = await client.PostAsJsonAsync("player-characters", dto);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
            throw new Exception($"Failed to add character: {response.ReasonPhrase}");
        }
    }

    public async Task UpdatePlayerCharacterAsync(PlayerCharacter character)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");

        var dto = new PlayerCharacterDTO
        {
            Id = character.Id,
            PlayerName = character.PlayerName,
            Name = character.Name,
            Race = character.Race,
            ImagePath = character.ImagePath,
            Strength = character.Strength,
            Dexterity = character.Dexterity,
            Constitution = character.Constitution,
            Intelligence = character.Intelligence,
            Wisdom = character.Wisdom,
            Charisma = character.Charisma,
            CurrentHP = character.CurrentHP,
            MaxHP = character.MaxHP,
            TempHP = character.TempHP,
            ArmorClass = character.ArmorClass,
            WalkingSpeed = character.WalkingSpeed,
            FlyingSpeed = character.FlyingSpeed,
            SwimmingSpeed = character.SwimmingSpeed,
            DarkvisionRange = character.DarkvisionRange,
            IsAlive = character.IsAlive,
            KnownLanguages = character.KnownLanguages.Select(lang => lang.ToString()).ToList(),
            Resistances = character.Resistances.Select(res => res.ToString()).ToList(),
            Weaknesses = character.Weaknesses.Select(weak => weak.ToString()).ToList(),
            HasOver20Stats = character.HasOver20Stats,
            CharacterClasses = character.CharacterClasses.Select(c => new CharacterClassDTO
            {
                Id = c.Id,
                ClassName = c.ClassName,
                Level = c.Level,
                PlayerCharacterId = c.PlayerCharacterId,
                DMCharacterId = c.DMCharacterId ?? null
            }).ToList()
        };

        var characterResponse = await client.PutAsJsonAsync($"player-characters/{character.Id}", dto);

        if (!characterResponse.IsSuccessStatusCode)
        {
            var errorContent = await characterResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating character: {characterResponse.StatusCode} - {errorContent}");
            throw new Exception($"Failed to update character: {characterResponse.ReasonPhrase} - {errorContent}");
        }
    }

    public async Task DeletePlayerCharacterAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        await client.DeleteAsync($"player-characters/{id}");
    }
}

