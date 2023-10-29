using Domain.Common;
using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
    
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlightDto>>> Get()
    {
        var result = await _flightService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<FlightDto>>> Get([FromQuery] FlightFilter filter)
    {
        var result = await _flightService.GetAllAsync(filter);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Policy = "Moderator")]
    public async Task<ActionResult<FlightDto>> Post([FromBody] FlightDto flightDto)
    {
        var result = await _flightService.AddAsync(flightDto);
        return Ok(result);
    }
    
    [HttpPut]
    [Authorize(Policy = "Moderator")]
    public async Task<IActionResult> Put([FromBody] FlightDto flightDto)
    {
        await _flightService.UpdateAsync(flightDto);
        return Ok();
    }
    
    [HttpDelete("{flightId}")]
    [Authorize(Policy = "Moderator")]
    public async Task<IActionResult> Delete(long flightId)
    {
        await _flightService.DeleteAsync(flightId);
        return Ok();
    }
}