using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("api/dm-characters")]
[ApiController]
public class DMCharacterController : ControllerBase
{
    private readonly IDMCharacterService _service;

    public DMCharacterController(IDMCharacterService service)
    {
        _service = service;
    }

    // GET: api/dm-characters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DMCharacter>>> GetCharacters()
    {
        return Ok(await _service.GetAllAsync());
    }

    // GET: api/dm-characters/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DMCharacter>> GetCharacter(int id)
    {
        var character = await _service.GetByIdAsync(id);

        if (character == null)
        {
            return NotFound();
        }

        return character;
    }

    // POST: api/dm-characters
    [HttpPost]
    public async Task<ActionResult<DMCharacter>> CreateCharacter(DMCharacter dmCharacter)
    {
        await _service.AddAsync(dmCharacter);

        return CreatedAtAction(nameof(GetCharacter), new { id = dmCharacter.Id }, dmCharacter);
    }

    // PUT: api/dm-characters/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCharacter(int id, DMCharacter dmCharacter)
    {
        if (id != dmCharacter.Id)
        {
            return BadRequest();
        }

        await _service.UpdateAsync(dmCharacter);

        return NoContent();
    }

    // DELETE: api/dm-characters/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDeleteCharacter(int id)
    {
        await _service.SoftDeleteAsync(id);
        return NoContent();
    }
}