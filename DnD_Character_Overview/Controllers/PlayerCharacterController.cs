using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Controllers;

[ApiController]
[Route("api/{version:apiVersion}/player-characters")]
public class PlayerCharacterController : ControllerBase
{
    // Services
    private readonly IPlayerCharacterService _service;
    private readonly IMapper _mapper; // Add AutoMapper

    // Constructor
    public PlayerCharacterController(IPlayerCharacterService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET: api/player-characters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerCharacterDTO>>> GetCharacters()
    {
        var characters = await _service.GetAllAsync();
        var characterDTOs = _mapper.Map<IEnumerable<PlayerCharacterDTO>>(characters);
        return Ok(characterDTOs);
    }

    // GET: api/player-characters/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerCharacterDTO>> GetCharacter(int id)
    {
        var character = await _service.GetByIdAsync(id);

        if (character == null)
        {
            return NotFound();
        }

        var characterDTO = _mapper.Map<PlayerCharacterDTO>(character);
        return Ok(characterDTO);
    }

    // POST: api/player-characters
    [HttpPost]
    public async Task<ActionResult<PlayerCharacterDTO>> CreateCharacter([FromBody] PlayerCharacterDTO playerCharacterDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var playerCharacter = _mapper.Map<PlayerCharacter>(playerCharacterDTO);
        await _service.AddAsync(playerCharacter);

        var createdCharacterDTO = _mapper.Map<PlayerCharacterDTO>(playerCharacter);
        return CreatedAtAction(nameof(GetCharacter), new { id = createdCharacterDTO.Id }, createdCharacterDTO);
    }

    // PUT: api/player-characters/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCharacter(int id, PlayerCharacterDTO playerCharacterDTO)
    {
        if (id != playerCharacterDTO.Id)
        {
            return BadRequest("Character ID mismatch.");
        }

        var playerCharacter = _mapper.Map<PlayerCharacter>(playerCharacterDTO);
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