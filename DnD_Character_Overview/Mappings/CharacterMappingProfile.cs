using AutoMapper;

public class CharacterMappingProfile : Profile
{
    public CharacterMappingProfile()
    {
        CreateMap<PlayerCharacter, PlayerCharacterDTO>().ReverseMap();
        CreateMap<DMCharacter, DMCharacterDTO>().ReverseMap();

        // Any additional mappings as needed
    }
}
