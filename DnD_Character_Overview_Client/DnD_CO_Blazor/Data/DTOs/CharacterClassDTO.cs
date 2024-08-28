namespace DTOs;

public class CharacterClassDTO
{
    public int Id { get; set; }
    public int? PlayerCharacterId { get; set; }
    public int? DMCharacterId { get; set; }
    public string? ClassName { get; set; }
    public int Level { get; set; }
}
