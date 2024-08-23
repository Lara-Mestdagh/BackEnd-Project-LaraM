using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class CharacterClass
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("PlayerCharacter")]
    public int PlayerCharacterId { get; set; }

    [Required]
    [StringLength(255)]
    public string? ClassName { get; set; }

    [Required]
    public int Level { get; set; }

    public PlayerCharacter? PlayerCharacter { get; set; }
}

// Since multi-classing is an option, 
// We seperate the class and level from the PlayerCharacter class