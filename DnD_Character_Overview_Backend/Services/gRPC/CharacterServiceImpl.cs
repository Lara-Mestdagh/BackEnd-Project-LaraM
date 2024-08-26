using Grpc.Core;
using System.Threading.Tasks;
using Protos;
using Google.Protobuf.Collections;
using System.Linq;
using System.Collections.Generic;

public class CharacterServiceImpl : Protos.CharacterService.CharacterServiceBase
{
    private readonly IPlayerCharacterService _characterService;

    public CharacterServiceImpl(IPlayerCharacterService characterService)
    {
        _characterService = characterService;
    }

    public override async Task<CharacterResponse> GetCharacter(GetCharacterRequest request, ServerCallContext context)
    {
        var character = await _characterService.GetByIdAsync(request.CharacterId);
        if (character == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Character not found."));
        }

        var response = new Protos.CharacterResponse
        {
            Character = new Protos.Character
            {
                Id = character.Id,
                Name = character.Name,
                Race = character.Race,
                PlayerName = character.PlayerName,
                ImagePath = character.ImagePath ?? string.Empty, // Handle null ImagePath
                Strength = character.Strength,
                Dexterity = character.Dexterity,
                Constitution = character.Constitution,
                Intelligence = character.Intelligence,
                Wisdom = character.Wisdom,
                Charisma = character.Charisma,
                MaxHp = character.MaxHP,
                CurrentHp = character.CurrentHP ?? 0,
                TempHp = character.TempHP,
                ArmorClass = character.ArmorClass,
                WalkingSpeed = character.WalkingSpeed,
                FlyingSpeed = character.FlyingSpeed ?? 0,
                SwimmingSpeed = character.SwimmingSpeed ?? 0,
                DarkvisionRange = character.DarkvisionRange ?? 0,
                InitiativeModifier = character.InitiativeModifier,
                Resistances = { character.Resistances != null ? character.Resistances.Select(r => r.ToString()) : Enumerable.Empty<string>() },
                Weaknesses = { character.Weaknesses != null ? character.Weaknesses.Select(w => w.ToString()) : Enumerable.Empty<string>() },
                Conditions = string.Join(", ", character.Conditions != null ? character.Conditions : Enumerable.Empty<string>()), 
                IsAlive = character.IsAlive,
                HasOver20Stats = character.HasOver20Stats,
                KnownLanguages = { character.KnownLanguages != null ? character.KnownLanguages.Select(l => l.ToString()) : Enumerable.Empty<string>() },
                CharacterClasses = { character.CharacterClasses != null ? character.CharacterClasses.Select(cc => new Protos.CharacterClass { Id = cc.Id, Name = cc.ClassName, Level = cc.Level }).ToList() : new List<Protos.CharacterClass>() }
            }
        };

        return response;
    }

    public override async Task<CharacterListResponse> GetAllCharacters(EmptyRequest request, ServerCallContext context)
    {
        var characters = await _characterService.GetAllAsync();
        var response = new CharacterListResponse();
        response.Characters.AddRange(characters.Select(c => new Protos.Character
        {
            Id = c.Id,
            Name = c.Name,
            Race = c.Race,
            PlayerName = c.PlayerName,
            ImagePath = c.ImagePath ?? string.Empty, // Handle null ImagePath
            Strength = c.Strength,
            Dexterity = c.Dexterity,
            Constitution = c.Constitution,
            Intelligence = c.Intelligence,
            Wisdom = c.Wisdom,
            Charisma = c.Charisma,
            MaxHp = c.MaxHP,
            CurrentHp = c.CurrentHP ?? 0,
            TempHp = c.TempHP,
            ArmorClass = c.ArmorClass,
            WalkingSpeed = c.WalkingSpeed,
            FlyingSpeed = c.FlyingSpeed ?? 0,
            SwimmingSpeed = c.SwimmingSpeed ?? 0,
            DarkvisionRange = c.DarkvisionRange ?? 0,
            InitiativeModifier = c.InitiativeModifier,
            Resistances = { c.Resistances != null ? c.Resistances.Select(r => r.ToString()) : Enumerable.Empty<string>() },
            Weaknesses = { c.Weaknesses != null ? c.Weaknesses.Select(w => w.ToString()) : Enumerable.Empty<string>() },
            Conditions = string.Join(", ", c.Conditions != null ? c.Conditions : Enumerable.Empty<string>()), 
            IsAlive = c.IsAlive,
            HasOver20Stats = c.HasOver20Stats,
            KnownLanguages = { c.KnownLanguages != null ? c.KnownLanguages.Select(l => l.ToString()) : Enumerable.Empty<string>() },
            CharacterClasses = { c.CharacterClasses != null ? c.CharacterClasses.Select(cc => new Protos.CharacterClass { Id = cc.Id, Name = cc.ClassName, Level = cc.Level }).ToList() : new List<Protos.CharacterClass>() }
        }));

        return response;
    }
}