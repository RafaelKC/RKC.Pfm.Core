using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RKC.Pfm.Core.Application.Communs;
using RKC.Pfm.Core.Application.Periods;
using RKC.Pfm.Core.Application.Periods.Dtos;
using RKC.Pfm.Core.Domain.Periods;

namespace RKC.Pfm.Core.Api.Periods;

[ApiController]
[Authorize]
[Route("periods")]
public class PeriodController: ControllerBase
{
    private readonly IPeriodService _periodService;

    public PeriodController(IPeriodService periodService)
    {
        _periodService = periodService;
    }

    [HttpGet]
    public async Task<PagedResult<PeriodDto>> GetList([FromQuery] PeriodGetListInput input)
    {
        return await _periodService.GetList(input);
    }
    
    [HttpGet("{periodId:guid}")]
    public async Task<ActionResult<PeriodDto>> GetList([FromRoute] Guid periodId)
    {
        var period = await _periodService.Get(periodId);
        if (period is null)
        {
            return NotFound();
        }

        return period;
    }
    
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PeriodDto input)
    {
        var created = await _periodService.Create(input);
        if (created)
        {
            return Created();
        }

        return UnprocessableEntity();
    }
    
    [HttpPut("{periodId:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid periodId, [FromBody] PeriodUpdateDto input)
    {
        var updated = await _periodService.Update(periodId, input);
        if (updated)
        {
            return Ok();
        }

        return UnprocessableEntity();
    }
    
    [HttpDelete("{periodId:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid periodId)
    {
        var deleted = await _periodService.Delete(periodId);
        if (deleted)
        {
            return Ok();
        }

        return UnprocessableEntity();
    }
}