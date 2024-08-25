using AutoMapper;

public class CharacterMappingProfile : Profile
{
    public CharacterMappingProfile()
    {
        // Map PlayerCharacterClass to PlayerCharacterClassDTO and vice versa
        CreateMap<PlayerCharacter, PlayerCharacterDTO>().ReverseMap();
        // Map DMCharacterClass to DMCharacterClassDTO and vice versa
        CreateMap<DMCharacter, DMCharacterDTO>().ReverseMap();
    }
}
