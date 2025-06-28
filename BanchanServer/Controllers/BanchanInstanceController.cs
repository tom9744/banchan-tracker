using BanchanServer.Models.Dtos;
using BanchanServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BanchanServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BanchanInstanceController(BanchanInstanceService banchanInstanceService) : ControllerBase
{
    private readonly BanchanInstanceService _banchanInstanceService = banchanInstanceService;

    [HttpPost]
    public async Task<ActionResult<BanchanInstanceDto>> CreateInstance(CreateBanchanInstanceDto dto)
    {
        try
        {
            var instance = await _banchanInstanceService.CreateInstance(dto.BanchanId, dto.Memo);

            return CreatedAtAction(
                actionName: nameof(GetInstanceById),
                routeValues: new { id = instance.Id },
                value: instance.ToDto()
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IEnumerable<BanchanInstanceDto>> GetInstances()
    {
        var instances = await _banchanInstanceService.GetInstances();

        return instances.Select(instance => instance.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BanchanInstanceDto>> GetInstanceById(string id)
    {
        var instance = await _banchanInstanceService.GetInstanceById(id);

        if (instance is null)
        {
            return NotFound();
        }

        return instance.ToDto();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BanchanInstanceDto>> UpdateInstance(string id, UpdateBanchanInstanceDto dto)
    {
        var updated = await _banchanInstanceService.UpdateInstanceMemo(id, dto.Memo);

        if (updated is null)
        {
            return NotFound();
        }

        return updated.ToDto();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInstance(string id)
    {
        var deleted = await _banchanInstanceService.DeleteInstance(id);

        if (deleted is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}