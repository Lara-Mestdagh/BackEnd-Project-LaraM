using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/dm-characters")]
[ApiVersion("2")]
[Authorize]
public class DMCharacterController : ControllerBase
{
    // Service
    private readonly IDMCharacterService _service;
    private readonly IMapper _mapper;

    // Constructor
    public DMCharacterController(IDMCharacterService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET: api/dm-characters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DMCharacterDTO>>> GetCharacters()
    {
        var characters = await _service.GetAllAsync();
        var characterDTOs = _mapper.Map<IEnumerable<DMCharacterDTO>>(characters);
        return Ok(characterDTOs);
    }

    // GET: api/dm-characters/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DMCharacterDTO>> GetCharacter(int id)
    {
        var character = await _service.GetByIdAsync(id);

        if (character == null)
        {
            return NotFound();
        }

        var characterDTO = _mapper.Map<DMCharacterDTO>(character);
        return Ok(characterDTO);
    }

    // POST: api/dm-characters
    [HttpPost]
    public async Task<ActionResult<DMCharacterDTO>> CreateCharacter(DMCharacterDTO dmCharacterDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dmCharacter = _mapper.Map<DMCharacter>(dmCharacterDTO);
        await _service.AddAsync(dmCharacter);

        var createdDMCharacterDTO = _mapper.Map<DMCharacterDTO>(dmCharacter);
        return CreatedAtAction(nameof(GetCharacter), new { id = createdDMCharacterDTO.Id}, createdDMCharacterDTO );
    }

    // PUT: api/dm-characters/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCharacter(int id, DMCharacterDTO dmCharacterDTO)
    {
        if (id != dmCharacterDTO.Id)
        {
            return BadRequest("Character ID mismatch.");
        }

        var dmCharacter = _mapper.Map<DMCharacter>(dmCharacterDTO);
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