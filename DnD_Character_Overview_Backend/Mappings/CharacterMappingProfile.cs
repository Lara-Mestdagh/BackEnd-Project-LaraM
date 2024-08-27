using AutoMapper;

public class CharacterMappingProfile : Profile
{
    public CharacterMappingProfile()
    {
        // Map CharacterClass to CharacterClassDTO and vice versa
        CreateMap<CharacterClass, CharacterClassDTO>().ReverseMap();
        // Map PlayerCharacterClass to PlayerCharacterClassDTO and vice versa
        CreateMap<PlayerCharacter, PlayerCharacterDTO>().ReverseMap();
        // Map DMCharacterClass to DMCharacterClassDTO and vice versa
        CreateMap<DMCharacter, DMCharacterDTO>().ReverseMap();
    }
}
