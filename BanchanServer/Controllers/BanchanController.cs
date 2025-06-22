using BanchanServer.Models.Dtos;
using BanchanServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BanchanServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BanchanController(BanchanService banchanService) : ControllerBase
{
    private readonly BanchanService _banchanService = banchanService;

    [HttpPost]
    public async Task<ActionResult<BanchanDto>> CreateBanchan(CreateBanchanDto dto)
    {
        try
        {
            var banchan = await _banchanService.CreateBanchan(dto.Name);

            return CreatedAtAction(
                actionName: nameof(GetBanchanById),
                routeValues: new { id = banchan.Id },
                value: banchan.ToDto()
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IEnumerable<BanchanDto>> GetBanchans()
    {
        var banchans = await _banchanService.GetBanchans();

        return banchans.Select(banchan => banchan.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BanchanDto>> GetBanchanById(string id)
    {
        var banchan = await _banchanService.GetBanchanById(id);

        if (banchan is null)
        {
            return NotFound();
        }

        return banchan.ToDto();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BanchanDto>> UpdateBanchan(string id, UpdateBanchanDto dto)
    {
        try 
        {
            var updated = await _banchanService.UpdateBanchan(id, dto.Name);

            if (updated is null)
            {
                return NotFound();
            }

            return updated.ToDto();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBanchan(string id)
    {
        var deleted = await _banchanService.DeleteBanchan(id);

        if (deleted is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}