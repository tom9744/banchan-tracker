using BanchanServer.Models.Dtos;
using BanchanServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BanchanServer.Controllers;

[ApiController]
[Route("api/BanchanInstance/{instanceId}/logs")]
public class BanchanInstanceLogController(BanchanInstanceLogService logService) : ControllerBase
{
    private readonly BanchanInstanceLogService _logService = logService;

    [HttpPost("consumption")]
    public async Task<ActionResult<BanchanInstanceLogDto>> CreateConsumptionLog(string instanceId, ConsumptionLogRequest dto)
    {
        try
        {
            var log = await _logService.ProcessConsumption(instanceId, dto.Portion);

            return CreatedAtAction(
                actionName: nameof(GetLogById),
                routeValues: new { instanceId, logId = log.Id },
                value: log.ToDto()
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

    [HttpPost("disposal")]
    public async Task<ActionResult<BanchanInstanceLogDto>> CreateDisposalLog(string instanceId)
    {
        try
        {
            var log = await _logService.ProcessDisposal(instanceId);

            return CreatedAtAction(
                actionName: nameof(GetLogById),
                routeValues: new { instanceId, logId = log.Id },
                value: log.ToDto()
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

    [HttpGet("{logId}")]
    public async Task<ActionResult<BanchanInstanceLogDto>> GetLogById(string logId)
    {
        var log = await _logService.GetLogById(logId);

        if (log is null)
        {
            return NotFound();
        }

        return log.ToDto();
    }
}