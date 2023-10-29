using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("userId")]
    public async Task<IActionResult> Get(long userId)
    {
        var result = await _userService.GetByIdAsync(userId);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> Post([FromBody] UserDto userDto)
    {
        var result = await _userService.AddAsync(userDto);
        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserDto userDto)
    {
        await _userService.UpdateAsync(userDto);
        return Ok();
    }
    
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(long userId)
    {
        await _userService.DeleteAsync(userId);
        return Ok();
    }
}