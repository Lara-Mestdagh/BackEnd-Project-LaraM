using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/player-characters")]
[ApiVersion("1.0")]
public class PlayerCharacterController : ControllerBase
{
    // Service
    private readonly IPlayerCharacterService _service;

    // Constructor
    public PlayerCharacterController(IPlayerCharacterService service)
    {
        _service = service;
    }

    // GET: api/player-characters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerCharacter>>> GetCharacters()
    {
        return Ok(await _service.GetAllAsync());
    }

    // GET: api/player-characters/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerCharacter>> GetCharacter(int id)
    {
        var character = await _service.GetByIdAsync(id);

        if (character == null)
        {
            return NotFound();
        }

        return character;
    }

    // POST: api/player-characters
    [HttpPost]
    public async Task<ActionResult<PlayerCharacter>> CreateCharacter(PlayerCharacter playerCharacter)
    {
        await _service.AddAsync(playerCharacter);

        return CreatedAtAction(nameof(GetCharacter), new { id = playerCharacter.Id }, playerCharacter);
    }

    // PUT: api/player-characters/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCharacter(int id, PlayerCharacter playerCharacter)
    {
        if (id != playerCharacter.Id)
        {
            return BadRequest("Character ID mismatch.");
        }

        await _service.UpdateAsync(playerCharacter);

        return NoContent();
    }

    // DELETE: api/player-characters/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDeleteCharacter(int id)
    {
        await _service.SoftDeleteAsync(id);
        return NoContent();
    }
}