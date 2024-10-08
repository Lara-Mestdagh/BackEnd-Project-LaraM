using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Controllers;

[ApiController]
[Route("api/{version:apiVersion}/files")]
public class FileController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IPlayerCharacterService _playerCharacterService;
    private readonly IDMCharacterService _dmCharacterService;

    public FileController(IInventoryService inventoryService, 
    IPlayerCharacterService playerCharacterService, 
    IDMCharacterService dMCharacterService)
    {
        _inventoryService = inventoryService;
        _playerCharacterService = playerCharacterService;
        _dmCharacterService = dMCharacterService;
    }

    // POST /api/files/upload – Upload a character image.
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCharacterImage(
        IFormFile file,
        [FromForm] int characterId,
        [FromForm] string type)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (characterId <= 0 || string.IsNullOrEmpty(type) || !(type == "player" || type == "dm"))
            return BadRequest("Invalid characterId or type.");

        try
        {
            // Generate a unique file name to prevent collisions
            var uniqueFileName = System.IO.Path.GetRandomFileName();
            var fileExtension = System.IO.Path.GetExtension(file.FileName);
            var filePath = System.IO.Path.Combine("../../BD-Project-LaraM/DnD_Character_Overview_Client/DnD_CO_Blazor/wwwroot/Images", uniqueFileName + fileExtension);

            // Ensure the directory exists
            var directory = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            
            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update the character's image path in the database
            if (type == "player")
            {
                var character = await _playerCharacterService.GetByIdAsync(characterId);
                if (character != null)
                {
                    character.ImagePath = filePath;
                    await _playerCharacterService.UpdateAsync(character);
                }
            }
            else if (type == "dm")
            {
                var character = await _dmCharacterService.GetByIdAsync(characterId);
                if (character != null)
                {
                    character.ImagePath = filePath;
                    await _dmCharacterService.UpdateAsync(character);
                }
            }

            return Ok(new { FilePath = filePath });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while uploading the file: {ex.Message}");
        }
    }


    // GET /api/files/download/inventory/{characterId}/{format} – Download a character’s inventory in the specified format (CSV or PDF).
    [HttpGet("download/inventory/{characterId}/{format}")]
    public async Task<IActionResult> DownloadInventory(int characterId, string format, [FromHeader(Name = "type")] string type)
    {
        if (string.IsNullOrEmpty(type) || !(type == "player" || type == "dm"))
        {
            return BadRequest("Invalid type. Must be 'player' or 'dm'.");
        }

        var inventoryItems = await _inventoryService.GetInventoryItemsForCharacterAsync(characterId, type);

        if (format.ToLower() == "csv")
        {
            var csv = GenerateCsv(inventoryItems);
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "inventory.csv");
        }
        else
        {
            return BadRequest("Unsupported format. Currently only CSV is supported.");
        }
    }

    // GET /api/files/download/party-inventory/{format} – Download the shared inventory in the specified format (CSV or PDF).
    [HttpGet("download/party-inventory/{format}")]
    public async Task<IActionResult> DownloadPartyInventory(string format)
    {
        var sharedInventory = await _inventoryService.GetSharedInventoryAsync();

        if (format.ToLower() == "csv")
        {
            var csv = GenerateCsv(sharedInventory.InventoryItems);
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "party-inventory.csv");
        }
        else
        {
            return BadRequest("Unsupported format. Currently only CSV is supported.");
        }
    }

    // GET /api/files/download/character-sheet/{characterId}/{format} – Download a character’s sheet in the specified format (CSV or PDF).
    // Currently only implemented for player characters
    [HttpGet("download/character-sheet/{characterId}/{format}")]
    public async Task<IActionResult> DownloadCharacterSheet(int characterId, string format)
    {
        var character = await _playerCharacterService.GetByIdAsync(characterId); 

        if (format.ToLower() == "csv")
        {
            var csv = GenerateCharacterCsv(character);
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "character-sheet.csv");
        }
        else
        {
            return BadRequest("Unsupported format. Currently only CSV is supported.");
        }
    }

    // Utility method to generate CSV from inventory items
    private string GenerateCsv(IEnumerable<InventoryItem> inventoryItems)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Id,ItemName,Quantity,Weight,WeightUnit");

        foreach (var item in inventoryItems)
        {
            csvBuilder.AppendLine($"{item.Id},{item.ItemName},{item.Quantity},{item.Weight},{item.WeightUnit}");
        }

        return csvBuilder.ToString();
    }

    // Utility method to generate CSV for a character
    private string GenerateCharacterCsv(PlayerCharacter character)
    {
        var csvBuilder = new StringBuilder();

        // Add basic character info
        csvBuilder.AppendLine("Character Information");
        csvBuilder.AppendLine("Name,Race,Class,Level");
        csvBuilder.AppendLine($"{character.Name},{character.Race},{character.CharacterClasses?.FirstOrDefault()?.ClassName ?? "N/A"},{character.CharacterClasses?.FirstOrDefault()?.Level ?? 0}");

        // Add headers for stats
        csvBuilder.AppendLine();
        csvBuilder.AppendLine("Character Stats");
        csvBuilder.AppendLine("Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma");
        csvBuilder.AppendLine($"{character.Strength},{character.Dexterity},{character.Constitution},{character.Intelligence},{character.Wisdom},{character.Charisma}");

        // Add headers for health and armor
        csvBuilder.AppendLine();
        csvBuilder.AppendLine("Health and Armor");
        csvBuilder.AppendLine("MaxHP,CurrentHP,TempHP,ArmorClass");
        csvBuilder.AppendLine($"{character.MaxHP},{character.CurrentHP ?? 0},{character.TempHP},{character.ArmorClass}");

        // Add headers for speed and vision
        csvBuilder.AppendLine();
        csvBuilder.AppendLine("Movement and Vision");
        csvBuilder.AppendLine("WalkingSpeed,FlyingSpeed,SwimmingSpeed,DarkvisionRange");
        csvBuilder.AppendLine($"{character.WalkingSpeed},{character.FlyingSpeed ?? 0},{character.SwimmingSpeed ?? 0},{character.DarkvisionRange ?? 0}");

        // Add headers for additional properties
        csvBuilder.AppendLine();
        csvBuilder.AppendLine("Additional Properties");
        csvBuilder.AppendLine("KnownLanguages,Resistances,Weaknesses,Conditions");

        // Format lists as readable strings
        string knownLanguages = character.KnownLanguages != null ? string.Join(";", character.KnownLanguages) : "None";
        string resistances = character.Resistances != null ? string.Join(";", character.Resistances) : "None";
        string weaknesses = character.Weaknesses != null ? string.Join(";", character.Weaknesses) : "None";
        string conditions = character.Conditions != null ? string.Join(";", character.Conditions) : "None";

        // Add additional properties as comma-separated lists
        csvBuilder.AppendLine($"{knownLanguages},{resistances},{weaknesses},{conditions}");

        return csvBuilder.ToString();
    }
}